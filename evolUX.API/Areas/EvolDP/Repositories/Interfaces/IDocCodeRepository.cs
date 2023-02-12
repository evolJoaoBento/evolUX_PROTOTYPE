using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.API.Areas.EvolDP.Repositories.Interfaces
{
    public interface IDocCodeRepository
    {
        public Task<IEnumerable<DocCode>> GetDocCodeGroup();
        public Task<IEnumerable<DocCode>> GetDocCode(string docLayout, string docType);
        public Task<IEnumerable<DocCodeConfig>> GetDocCodeConfig(int docCodeID);
        public Task<DocCodeConfig> GetDocCodeConfig(int docCodeID, int startdate);
        public Task<DocCodeConfig> GetDocCodeConfigOptions(int docCodeID);
        public Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel1();
        public Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel2();
        public Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel3();
        public Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMediaGroups(string envMediaGroupID);
        public Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMediaGroups();
        public Task<IEnumerable<int>> GetAggregationList(string aggrCompatibility);
        public Task<IEnumerable<Company>> GetExpeditionCompanies(string expCompanyID);
        public Task<IEnumerable<Company>> GetExpeditionCompanies();
        public Task<IEnumerable<ExpeditionsType>> GetExpeditionTypes(string expeditionType);
        public Task<IEnumerable<ExpeditionsType>> GetExpeditionTypes();
        public Task<IEnumerable<ServiceTask>> GetTreatmentTypes(string treatmentType);
        public Task<IEnumerable<ServiceTask>> GetTreatmentTypes();
        public Task<IEnumerable<int>> GetFinishingList(string finishing);
        public Task<IEnumerable<int>> GetArchiveList(string archive);
        public Task<IEnumerable<Email>> GetEmailList(string email);
        public Task<IEnumerable<Email>> GetEmailList();
        public Task<IEnumerable<int>> GetEmailHideList(string emailHide);
        public Task<IEnumerable<Electronic>> GetElectronicList(string electronic);
        public Task<IEnumerable<Electronic>> GetElectronicList();
        public Task<IEnumerable<int>> GetElectronicHideList(string electronicHide);
        public Task PostDocCodeConfig(DocCodeConfig model);
        public Task<IEnumerable<string>> DeleteDocCode(int docCodeID);
        Task<IEnumerable<AggregateDocCode>> GetAggregateDocCodes(int docCodeID);
        Task<AggregateDocCode> GetAggregateDocCode(int docCodeID);
        Task ChangeCompatibility(DocCodeCompatabilityViewModel model);
    }
}
