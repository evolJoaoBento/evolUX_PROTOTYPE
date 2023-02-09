using evolUX.API.Areas.Finishing.Services.Interfaces;
using System.Data;
using Shared.Models.General;
using Shared.ViewModels.General;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using evolUX.API.Extensions;
using Shared.Models.Areas.Core;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using System.Xml;

namespace evolUX.API.Areas.Finishing.Services
{
    public class PrintService : IPrintService
    {
        private readonly IWrapperRepository _repository;
        public PrintService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        public void GetPrinterFeatures(string specs, ref int colorFeature, ref int plexFeature)
        {
            GetPrinterFeatures(specs, "", ref colorFeature, ref plexFeature);
        }

        public void GetPrinterFeatures(string specs, string plexCode, ref int colorFeature, ref int plexFeature)
        {
            //Check Print Service Types
            string xmlString = "<?xml version='1.0'?><root>" + specs + "</root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            XmlNodeList? nodes = doc.SelectNodes("/root/PRINTSERVICETYPE");
            if (nodes != null && nodes.Count > 0)
            {
                foreach (XmlNode node in nodes)
                {
                    if (!string.IsNullOrEmpty(node.InnerText))
                    {
                        if (node.InnerText.StartsWith("PRINTCOLOR"))
                            colorFeature = colorFeature | 2;
                        if (node.InnerText.StartsWith("PRINTBLACK"))
                            colorFeature = colorFeature | 1;
                        if (node.InnerText.EndsWith("SPLEX") || (!string.IsNullOrEmpty(plexCode) && plexCode == "SPLEX"))
                            plexFeature = plexFeature | 1;
                        else if (node.InnerText.EndsWith("DPLEX") || (!string.IsNullOrEmpty(plexCode) && plexCode == "DPLEX"))
                            plexFeature = plexFeature | 2;
                    }
                }
            }
            if (plexFeature == 0) //O serviço num impressora simplex ou duplex é igual por isso é indiferente onde se imprime
                plexFeature = 3;
        }

        public async Task<PrinterViewModel> GetPrinters(IEnumerable<int> profileList, string filesSpecs, bool ignoreProfiles)
        {
            PrinterViewModel viewModel = new PrinterViewModel();
            IEnumerable<ResourceInfo> result = await _repository.RegistJob.GetResources("PRINTER", profileList, filesSpecs, ignoreProfiles);
            if (result != null)
            {
                List<PrinterInfo> Printers = new List<PrinterInfo>();
                foreach (ResourceInfo r in result)
                {
                    PrinterInfo p = new PrinterInfo();
                    p.ResValue = r.ResValue;
                    p.MatchFilter = r.MatchFilter;
                    p.ResID = r.ResID;
                    p.Description = r.Description;
                    int colorFeature = 0;
                    int plexFeature = 0;

                    GetPrinterFeatures(p.ResValue, ref colorFeature, ref plexFeature);
                    p.ColorFeature = colorFeature;
                    p.PlexFeature = plexFeature;

                    Printers.Add(p);
                }
                viewModel.Printers = Printers;
            }
            return viewModel;
        }

        public async Task<Result> Print(string printer, string serviceCompanyCode, string username, int userID, List<PrintFileInfo> prodFiles)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("SERVICECOMPANYCODE", serviceCompanyCode);
            dictionary.Add("TYPE", "PRINT");

            Result viewmodel = new Result();
            if (prodFiles == null || prodFiles.Count == 0)
            {
                viewmodel.Error = "No Files to Print";
                return viewmodel;
            }

            FlowInfo flowinfo = await _repository.RegistJob.GetFlowByCriteria(dictionary);

            if (flowinfo != null)
            {
                foreach (PrintFileInfo f in prodFiles)
                {
                    flowinfo.FlowName = f.FileName + " [" + flowinfo.FlowName + "]";

                    IEnumerable<FlowParameter> flowparameters = await _repository.RegistJob.GetFlowData(flowinfo.FlowID);
                    string query;
                    if (f.PrintRecNumber < 0)
                        query = "<START>EXEC RT_INSERT_INTO_FILE_LOG @RunID = @RUNID, @FileID = @FILEID, @RunStateName = ''SEND2PRINTER''</START><END>EXEC RT_UPDATE_FILE_LOG_ENDTIMESTAMP @RunID = @RUNID, @FileID = @FILEID, @RunStateName = ''SEND2PRINTER'', @ProcCountNr = @PROCCOUNTNR, @OutputPath = ''@PRINTERNAME'', @OutputName = ''@USERNAME''</END>";
                    else
                        query = string.Format("<END>SET NOCOUNT ON UPDATE RT_EXPCOMPANY_REGIST_DETAIL_FILE SET PrintedTimeStamp = CURRENT_TIMESTAMP WHERE RunID = @RUNID AND FileID = @FILEID AND RecNumber = {0} SELECT * FROM RT_EXPCOMPANY_REGIST_DETAIL_FILE WHERE RunID = @RUNID AND FileID = @FILEID AND RecNumber = {0}</END>", f.PrintRecNumber);

                    foreach (FlowParameter p in flowparameters)
                    {
                        p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/FILE/FILEPATH ", f.FilePath);
                        p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/FILE/FILENAME ", f.FileName);
                        p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/FILE/RUNID ", f.RunID.ToString());
                        p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/FILE/FILEID ", f.FileID.ToString());
                        p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/FILE/SHORTFILENAME ", f.ShortFileName);
                        p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/PRINTER ", printer);
                        p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/SYSTEM/LOGUSER ", username);
                        p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/QUERY ", query);
                    }
                    viewmodel = await _repository.RegistJob.TryRegistJob(flowparameters, flowinfo, userID);
                    if (viewmodel == null)
                    {
                        throw new NullReferenceException("No result was sent by the Database!");
                    }
                    if (f.PrintRecNumber < 0)
                        await _repository.ProductionReport.LogSentToPrinter(f.RunID, f.FileID);
                }
            }
            else
            {
                viewmodel.Error = "No Flow found";
            }
            return viewmodel;
        }        
    }
}
