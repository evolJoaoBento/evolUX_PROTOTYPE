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

        public async Task<Result> Print(int runID, int fileID, string printer, string serviceCompanyCode,
            string username, int userID, string filePath, string fileName, string shortFileName)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("SERVICECOMPANYCODE", serviceCompanyCode);
            dictionary.Add("TYPE", "PRINT");

            FlowInfo flowinfo = await _repository.RegistJob.GetFlowByCriteria(dictionary);
            flowinfo.FlowName = fileName + " [" + flowinfo.FlowName + "]";

            IEnumerable<FlowParameter> flowparameters = await _repository.RegistJob.GetFlowData(flowinfo.FlowID);

            string query = "<START>EXEC RT_INSERT_INTO_FILE_LOG @RunID = @RUNID, @FileID = @FILEID, @RunStateName = ''SEND2PRINTER''</START>\r\n<END>EXEC RT_UPDATE_FILE_LOG_ENDTIMESTAMP @RunID = @RUNID, @FileID = @FILEID, @RunStateName = ''SEND2PRINTER'', @ProcCountNr = @PROCCOUNTNR, @OutputPath = ''@PRINTERNAME'', @OutputName = ''@USERNAME''</END>\r\n";

            foreach (FlowParameter p in flowparameters)
            {
                p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/FILE/FILEPATH ", filePath);
                p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/FILE/FILENAME ", fileName);
                p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/FILE/RUNID ", runID.ToString());
                p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/FILE/FILEID ", fileID.ToString());
                p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/FILE/SHORTFILENAME ", shortFileName);
                p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/PRINTER ", printer);
                p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/SYSTEM/LOGUSER ", username);
                p.ParameterValue = p.ParameterValue.Replace("@PARAMETERS/ACTION/QUERY ", query);
                //p.ParameterValue = p.ParameterValue.Replace("select '", "");
                //p.ParameterValue = p.ParameterValue.Replace("SELECT '", "");
                //p.ParameterValue = p.ParameterValue.Replace(" '", "");
            }
            Result viewmodel = await _repository.RegistJob.TryRegistJob(flowparameters, flowinfo, userID);
            if (viewmodel == null)
            {
                throw new NullReferenceException("No result was sent by the Database!");
            }
            await _repository.ProductionReport.LogSentToPrinter(runID, fileID);
            return viewmodel;
        }        
    }
}
