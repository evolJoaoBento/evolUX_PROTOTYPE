using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using Shared.ViewModels.Areas.evolDP;
using Shared.Models.Areas.evolDP;
using System.Security.Cryptography.Xml;
using System.Globalization;
using Microsoft.SqlServer.Server;
using Shared.Models.General;
using System.Data;
using System.Diagnostics;

namespace evolUX.API.Areas.evolDP.Services
{
    public class DocCodeService : IDocCodeService
    {
        //TODO VIEWMODEL RETURNS
        private readonly IWrapperRepository _repository;
        public DocCodeService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        public async Task<DocCodeViewModel> GetDocCodeGroup()
        {
            DocCodeViewModel viewmodel = new DocCodeViewModel();
            viewmodel.DocCodeList = await _repository.DocCode.GetDocCodeGroup();
            return viewmodel;
        }

        public async Task<DocCodeViewModel> GetDocCode(string docLayout, string docType)
        {
            DocCodeViewModel viewmodel = new DocCodeViewModel();
            viewmodel.DocCodeList = await _repository.DocCode.GetDocCode(docLayout, docType, -1);
            //foreach(DocCode doc in viewmodel.DocCodeList) 
            //{
            //    doc.DocCodeConfigs = (await _repository.DocCode.GetDocCodeConfig(doc.DocCodeID, (int?)null, null)).ToList();
            //}
            return viewmodel;
        }

        public async Task<DocCodeConfigViewModel> GetDocCodeConfig(int docCodeID, DateTime? startDate, bool? maxDateFlag)
        {
            DocCodeConfigViewModel viewmodel = new DocCodeConfigViewModel();
            viewmodel.DocCode = new DocCode();
            viewmodel.DocCode.DocCodeID = docCodeID;
            viewmodel.DocCode.DocCodeConfigs = (await _repository.DocCode.GetDocCodeConfig(docCodeID, startDate, null)).ToList();
            return viewmodel;
        }

        public async Task<DocCodeConfigOptionsViewModel> GetDocCodeConfigOptions(DocCode? docCode)
        {
            DocCodeConfigOptionsViewModel viewmodel = new DocCodeConfigOptionsViewModel();
            viewmodel.DocCodeConfig = new DocCode();
            if (docCode != null)
            {
                if (docCode.DocCodeID > 0 || !string.IsNullOrEmpty(docCode.DocLayout))
                {
                    viewmodel.DocCodeConfig = docCode;
                    if (docCode.DocCodeID <= 0)
                    {
                        DocCodeViewModel docView = new DocCodeViewModel();
                        docView.DocCodeList = await _repository.DocCode.GetDocCode(docCode.DocLayout,  docCode.DocType, 1);
                        if (docView.DocCodeList == null || docView.DocCodeList.Count() == 0)
                            viewmodel.DocCodeConfig = new DocCode();
                        else
                            viewmodel.DocCodeConfig = docView.DocCodeList?.First();
                    }
                }
                if (viewmodel.DocCodeConfig.DocCodeID > 0)
                {
                    DateTime sDate;
                    DateTime? startDate = null;
                    if (viewmodel.DocCodeConfig.DocCodeConfigs != null && viewmodel.DocCodeConfig.DocCodeConfigs.Count > 0)
                    {
                        if (DateTime.TryParseExact(viewmodel.DocCodeConfig.DocCodeConfigs[0].StartDate.ToString(), "yyyyMMdd", 
                            System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out sDate))
                            startDate = sDate;
                    }
                    viewmodel.DocCodeConfig.DocCodeConfigs = (await _repository.DocCode.GetDocCodeConfig(viewmodel.DocCodeConfig.DocCodeID, startDate, true)).ToList();
                    if (startDate!= null) { viewmodel.DocCodeConfig.DocCodeConfigs[0].IsEditable = true; }
                }
            }
            //Recolhe as outras configs
            viewmodel.Exceptionslevel1List = await _repository.DocCode.GetExceptionLevel(1);
            viewmodel.Exceptionslevel2List = await _repository.DocCode.GetExceptionLevel(2);
            viewmodel.Exceptionslevel3List = await _repository.DocCode.GetExceptionLevel(3);
            viewmodel.EnvMediaGroups = await _repository.DocCode.GetEnvelopeMediaGroups(null);
            viewmodel.AggregationList = await _repository.DocCode.GetAggregationList();
            viewmodel.ExpeditionTypes = await _repository.DocCode.GetExpeditionTypes(null);
            viewmodel.ExpCodeList = await _repository.DocCode.GetExpCompanyServiceTask("");
            viewmodel.ServiceTasks = await _repository.DocCode.GetServiceTasks(null);
            return viewmodel;
        }

        public async Task<ExceptionLevelViewModel> GetExceptionLevel(int level)
        {
            ExceptionLevelViewModel viewmodel = new ExceptionLevelViewModel();
            viewmodel.Level = level;
            viewmodel.ExceptionslevelList = await _repository.DocCode.GetExceptionLevel(level);
            return viewmodel;
        }

        public async Task<ExceptionLevelViewModel> SetExceptionLevel(int level, int exceptionID, string exceptionCode, string exceptionDescription)
        {
            ExceptionLevelViewModel viewmodel = new ExceptionLevelViewModel();
            viewmodel.Level = level;
            viewmodel.ExceptionslevelList = await _repository.DocCode.SetExceptionLevel(level, exceptionID, exceptionCode, exceptionDescription);
            return viewmodel;
        }

        public async Task<ExceptionLevelViewModel> DeleteExceptionLevel(int level, int exceptionID)
        {
            ExceptionLevelViewModel viewmodel = new ExceptionLevelViewModel();
            viewmodel.Level = level;
            viewmodel.ExceptionslevelList = await _repository.DocCode.DeleteExceptionLevel(level, exceptionID);
            return viewmodel;
        }
        
        
        public async Task<DocCodeViewModel> SetDocCodeConfig(DocCode docCode)
        {
            DocCodeViewModel viewmodel = new DocCodeViewModel();
            viewmodel.DocCodeList = await _repository.DocCode.SetDocCodeConfig(docCode);
            return viewmodel;
        }

        public async Task<DocCodeViewModel> ChangeDocCode(DocCode docCode)
        {
            DocCodeViewModel viewmodel = new DocCodeViewModel();
            viewmodel.DocCodeList = await _repository.DocCode.ChangeDocCode(docCode);
            return viewmodel;
        }

        public async Task<Result> DeleteDocCode(int docCodeID)
        {
            Result results = await _repository.DocCode.DeleteDocCode(docCodeID);
            if (results == null)
            {

            }
            return results;

        }

        public async Task<Result> DeleteDocCodeConfig(int docCodeID, int startDate)
        {
            Result results = await _repository.DocCode.DeleteDocCodeConfig(docCodeID, startDate);
            if (results == null)
            {

            }
            return results;
        }
        public async Task<IEnumerable<AggregateDocCode>> GetCompatibility(int docCodeID)
        {
            IEnumerable<AggregateDocCode> aggList = await _repository.DocCode.GetCompatibility(docCodeID);
            if (aggList == null)
            {

            }
            return aggList;
        }
        public async Task<IEnumerable<AggregateDocCode>> ChangeCompatibility(int docCodeID, DataTable docCodeList)
        {
            IEnumerable<AggregateDocCode> aggList = await _repository.DocCode.ChangeCompatibility(docCodeID, docCodeList);
            if (aggList == null)
            {

            }
            return aggList;
        }
        public async Task<DocCodeData4ScriptViewModel> DocCodeData4Script(int docCodeID, int startDate)
        {
            DocCodeData4ScriptViewModel results = await _repository.DocCode.DocCodeData4Script(docCodeID, startDate);
            if (results == null)
            {

            }
            return results;
        }
    }
}
