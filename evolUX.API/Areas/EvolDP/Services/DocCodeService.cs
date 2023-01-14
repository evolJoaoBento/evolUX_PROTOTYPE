using evolUX.API.Areas.EvolDP.Models;
using evolUX.API.Areas.EvolDP.Services.Interfaces;
using evolUX.API.Areas.EvolDP.ViewModels;
using evolUX.API.Data.Interfaces;

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
            viewmodel.DocCodeList = await _repository.DocCode.GetDocCode(docLayout, docType);
            return viewmodel;
        }        
        public async Task<DocCodeConfigViewModel> GetDocCodeConfig(string ID)
        {
            DocCodeConfigViewModel viewmodel = new DocCodeConfigViewModel();
            viewmodel.DocCodeConfigList = await _repository.DocCode.GetDocCodeConfig(ID);
            return viewmodel;
        }        

        public async Task<DocCodeConfig> GetDocCodeConfig(string ID, int startdate)
        {
            DocCodeConfig docCodeConfig = await _repository.DocCode.GetDocCodeConfig(ID,startdate);
            if (docCodeConfig == null)
            {

            }
            return docCodeConfig;
        }

        public async Task<DocCodeConfig> GetDocCodeConfigOptions(string ID)
        {
            DocCodeConfig docCodeConfig = await _repository.DocCode.GetDocCodeConfigOptions(ID);
            if (docCodeConfig == null)
            {

            }
            return docCodeConfig;
        }

        public async Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel1()
        {
            IEnumerable<ExceptionLevel> docExceptionList = await _repository.DocCode.GetDocExceptionsLevel1();
            if (docExceptionList == null)
            {

            }
            return docExceptionList;
        }

        public async Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel2()
        {
            IEnumerable<ExceptionLevel> docExceptionList = await _repository.DocCode.GetDocExceptionsLevel2();
            if (docExceptionList == null)
            {

            }
            return docExceptionList;
        }

        public async Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel3()
        {
            IEnumerable<ExceptionLevel> docExceptionList = await _repository.DocCode.GetDocExceptionsLevel3();
            if (docExceptionList == null)
            {

            }
            return docExceptionList;
        }

        public async Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMediaGroups(string envMediaGroupID)
        {
            if(envMediaGroupID == null)
            {
                IEnumerable<EnvelopeMedia> envMediaList = await _repository.DocCode.GetEnvelopeMediaGroups();
                if (envMediaList == null)
                {

                }
                return envMediaList;
            }
            else
            {
                IEnumerable<EnvelopeMedia> envMediaList = await _repository.DocCode.GetEnvelopeMediaGroups(envMediaGroupID);
                if (envMediaList == null)
                {

                }
                return envMediaList;
            }
            
            
        }

        public async Task<IEnumerable<int>> GetAggregationList(string aggrCompatibility)
        {
            IEnumerable<int> aggregationList = await _repository.DocCode.GetAggregationList(aggrCompatibility);
            if (aggregationList == null)
            {

            }
            return aggregationList;
        }

        public async Task<IEnumerable<Company>> GetExpeditionCompanies(string companyName)
        {
            if(companyName == null)
            {
                IEnumerable<Company> companyList = await _repository.DocCode.GetExpeditionCompanies();
                if (companyList == null)
                {

                }
                return companyList;
            }
            else
            {
                IEnumerable<Company> companyList = await _repository.DocCode.GetExpeditionCompanies(companyName);
                if (companyList == null)
                {

                }
                return companyList;
            }
            
        }

        public async Task<IEnumerable<ExpeditionsType>> GetExpeditionTypes(string expeditionType)
        {
            if (expeditionType==null)
            {
                IEnumerable<ExpeditionsType> expeditionTypeList = await _repository.DocCode.GetExpeditionTypes();
                if (expeditionTypeList == null)
                {

                }
                return expeditionTypeList;
            }
            else
            {
                IEnumerable<ExpeditionsType> expeditionTypeList = await _repository.DocCode.GetExpeditionTypes(expeditionType);
                if (expeditionTypeList == null)
                {

                }
                return expeditionTypeList;
            }
        }
         

        public async Task<IEnumerable<TreatmentType>> GetTreatmentTypes(string treatmentType)
        {
            if (treatmentType==null)
            {
                IEnumerable<TreatmentType> treatmentTypeList = await _repository.DocCode.GetTreatmentTypes();
                if (treatmentTypeList == null)
                {

                }
                return treatmentTypeList;
            }
            else
            {
                IEnumerable<TreatmentType> treatmentTypeList = await _repository.DocCode.GetTreatmentTypes(treatmentType);
                if (treatmentTypeList == null)
                {

                }
                return treatmentTypeList;
            }
            
        }

        public async Task<IEnumerable<int>> GetFinishingList(string finishing)
        {
            IEnumerable<int> finishingList = await _repository.DocCode.GetFinishingList(finishing);
            if (finishingList == null)
            {

            }
            return finishingList;
        }

        public async Task<IEnumerable<int>> GetArchiveList(string archive)
        {
            IEnumerable<int> archiveList = await _repository.DocCode.GetArchiveList(archive);
            if (archiveList == null)
            {

            }
            return archiveList;
        }

        public async Task<IEnumerable<Email>> GetEmailList(string email)
        {
            IEnumerable<Email> emailList = await _repository.DocCode.GetEmailList(email);
            if (emailList == null)
            {

            }
            return emailList;
        }

        public async Task<IEnumerable<int>> GetEmailHideList(string emailHide)
        {
            IEnumerable<int> emailHideList = await _repository.DocCode.GetEmailHideList(emailHide);
            if (emailHideList == null)
            {

            }
            return emailHideList;
        }

        public async Task<IEnumerable<Electronic>> GetElectronicList(string electronic)
        {
            IEnumerable<Electronic> electronicList = await _repository.DocCode.GetElectronicList(electronic);
            if (electronicList == null)
            {

            }
            return electronicList;
        }

        public async Task<IEnumerable<int>> GetElectronicHideList(string electronicHide)
        {
            IEnumerable<int> electronicHideList = await _repository.DocCode.GetElectronicHideList(electronicHide);
            if (electronicHideList == null)
            {

            }
            return electronicHideList;
        }
        public async Task PostDocCodeConfig(DocCodeConfig model)
        {
            await _repository.DocCode.PostDocCodeConfig(model);
            

        }
        public async Task<IEnumerable<string>> DeleteDocCode(string ID)
        {
            IEnumerable<string>  results = await _repository.DocCode.DeleteDocCode(ID);
            if (results == null)
            {

            }
            return results;

        }


        public async Task<IEnumerable<AggregateDocCode>> GetAggregateDocCodes(string ID)
        {
            IEnumerable<AggregateDocCode> aggregateDocCodes = await _repository.DocCode.GetAggregateDocCodes(ID);
            if (aggregateDocCodes == null)
            {

            }
            return aggregateDocCodes;
        }

        public async Task<AggregateDocCode> GetAggregateDocCode(string ID)
        {
            AggregateDocCode aggregateDocCode = await _repository.DocCode.GetAggregateDocCode(ID);
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
