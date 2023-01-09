using Shared.Models.Areas.Finishing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using evolUX.API.Data.Interfaces;
using System.Data;
using Shared.Models.General;
using Shared.ViewModels.General;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using evolUX.API.Extensions;

namespace evolUX.API.Areas.Finishing.Services
{
    public class PrintService : IPrintService
    {
        private readonly IWrapperRepository _repository;
        public PrintService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResoursesViewModel> GetPrinters(IEnumerable<int> profileList, string filesSpecs, bool ignoreProfiles)
        {
            ResoursesViewModel viewmodel = new ResoursesViewModel();
            viewmodel.Resources = await _repository.PrintFiles.GetPrinters(profileList, filesSpecs, ignoreProfiles);
            if (viewmodel.Resources == null)
            {

            }
            return viewmodel;
        }

        public async Task<Result> Print(int runID, int fileID, string printer, string serviceCompanyCode, 
            string username, int userID, string filePath, string fileName, string shortFileName)
        {
            FlowInfo flowinfo = await _repository.PrintFiles.GetFlow(serviceCompanyCode);
            flowinfo.FlowName = fileName + " [" + flowinfo.FlowName + "]";
            IEnumerable<FlowParameter> flowparameters = await _repository.PrintFiles.GetFlowParameters(flowinfo.FlowID);

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
            Result viewmodel = await _repository.PrintFiles.TryPrint(flowparameters, flowinfo, userID);
            if (viewmodel == null)
            {
                throw new NullReferenceException("No result was sent by the Database!");
            }
            await _repository.PrintFiles.LogSentToPrinter(runID, fileID);
            return viewmodel;
        }
    }
}
