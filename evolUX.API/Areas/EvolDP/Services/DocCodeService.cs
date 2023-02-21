using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.EvolDP.Services.Interfaces;
using Shared.ViewModels.Areas.evolDP;
using Shared.Models.Areas.evolDP;
using System.Security.Cryptography.Xml;
using System.Globalization;
using Microsoft.SqlServer.Server;
using Shared.Models.General;

namespace evolUX.API.Areas.EvolDP.Services
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
            foreach(DocCode doc in viewmodel.DocCodeList) 
            {
                doc.DocCodeConfigs = (await _repository.DocCode.GetDocCodeConfig(doc.DocCodeID, (int?)null, null)).ToList();
            }
            return viewmodel;
        }

        public async Task<DocCodeViewModel> GetDocCodeConfig(int docCodeID, DateTime? startDate, bool? maxDateFlag)
        {
            DocCodeViewModel viewmodel = new DocCodeViewModel();
            List<DocCode> docCodeList = new List<DocCode>();
            docCodeList.Add(new DocCode() { DocCodeID = docCodeID});
            docCodeList[0].DocCodeConfigs = (await _repository.DocCode.GetDocCodeConfig(docCodeID, startDate, null)).ToList();
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
            viewmodel.Exceptionslevel1List = await _repository.DocCode.GetDocExceptionsLevel(1);
            viewmodel.Exceptionslevel2List = await _repository.DocCode.GetDocExceptionsLevel(2);
            viewmodel.Exceptionslevel3List = await _repository.DocCode.GetDocExceptionsLevel(3);
            viewmodel.EnvMediaGroups = await _repository.DocCode.GetEnvelopeMediaGroups(null);
            viewmodel.AggregationList = await _repository.DocCode.GetAggregationList();
            viewmodel.ExpeditionTypes = await _repository.DocCode.GetExpeditionTypes(null);
            viewmodel.ExpCodeList = await _repository.DocCode.GetExpCompanyServiceTask("");
            viewmodel.ServiceTasks = await _repository.DocCode.GetServiceTasks(null);
            viewmodel.SuportTypeList = await _repository.DocCode.GetSuporTypeOptionList();
            if (viewmodel.SuportTypeList != null)
            {
                List<GenericOptionValue> optionList = new List<GenericOptionValue>();
                if (viewmodel.SuportTypeList.List != null && viewmodel.SuportTypeList.List.Count() > 0)
                {
                    List<string> options = viewmodel.SuportTypeList.List.Select(x => x.GroupCode).Distinct().ToList();
                    foreach (string option in options)
                    {
                        optionList.Add(new GenericOptionValue()
                        {
                            ID = viewmodel.SuportTypeList.List.Where(x => x.GroupCode == option && x.ID != 0).Min(x => x.ID),
                            Code = option,
                            GroupCode = option
                        });
                    }
                }

                viewmodel.SuportTypeList.OptionList = optionList;
            }
            return viewmodel;
        }

        public async Task<DocCodeViewModel> PostDocCodeConfig(DocCode docCode)
        {
            DocCodeViewModel viewmodel = new DocCodeViewModel();
            viewmodel.DocCodeList = await _repository.DocCode.PostDocCodeConfig(docCode);
            return viewmodel;
        }

        public async Task<IEnumerable<string>> DeleteDocCode(int docCodeID)
        {
            IEnumerable<string>  results = await _repository.DocCode.DeleteDocCode(docCodeID);
            if (results == null)
            {

            }
            return results;

        }

        public async Task<IEnumerable<AggregateDocCode>> GetAggregateDocCodes(int docCodeID)
        {
            IEnumerable<AggregateDocCode> aggregateDocCodes = await _repository.DocCode.GetAggregateDocCodes(docCodeID);
            if (aggregateDocCodes == null)
            {

            }
            return aggregateDocCodes;
        }

        public async Task<AggregateDocCode> GetAggregateDocCode(int docCodeID)
        {
            AggregateDocCode aggregateDocCode = await _repository.DocCode.GetAggregateDocCode(docCodeID);
            if (aggregateDocCode == null)
            {

            }
            return aggregateDocCode;
        }

        public async Task ChangeCompatibility(DocCodeCompatabilityViewModel model)
        {
            await _repository.DocCode.ChangeCompatibility(model);
        }
    }
}
