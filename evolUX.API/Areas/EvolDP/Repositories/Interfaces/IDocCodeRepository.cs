using evolUX.API.Areas.EvolDP.Models;
using evolUX.API.Areas.EvolDP.ViewModels;
using Shared.Models.Areas.evolDP;

namespace evolUX.API.Areas.EvolDP.Repositories.Interfaces
{
    public interface IDocCodeRepository
    {
        public Task<IEnumerable<DocCode>> GetDocCodeGroup();
        public Task<IEnumerable<DocCode>> GetDocCode(string docLayout, string docType);
        public Task<IEnumerable<DocCodeConfig>> GetDocCodeConfig(string ID);
        public Task<DocCodeConfig> GetDocCodeConfig(string ID, int startdate);
        public Task<DocCodeConfig> GetDocCodeConfigOptions(string ID);
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
        public Task<IEnumerable<TreatmentType>> GetTreatmentTypes(string treatmentType);
        public Task<IEnumerable<TreatmentType>> GetTreatmentTypes();
        public Task<IEnumerable<int>> GetFinishingList(string finishing);
        public Task<IEnumerable<int>> GetArchiveList(string archive);
        public Task<IEnumerable<Email>> GetEmailList(string email);
        public Task<IEnumerable<Email>> GetEmailList();
        public Task<IEnumerable<int>> GetEmailHideList(string emailHide);
        public Task<IEnumerable<Electronic>> GetElectronicList(string electronic);
        public Task<IEnumerable<Electronic>> GetElectronicList();
        public Task<IEnumerable<int>> GetElectronicHideList(string electronicHide);
        public Task PostDocCodeConfig(DocCodeConfig model);
        public Task<IEnumerable<string>> DeleteDocCode(string ID);
        Task<IEnumerable<AggregateDocCode>> GetAggregateDocCodes(string ID);
        Task<AggregateDocCode> GetAggregateDocCode(string ID);
        Task ChangeCompatibility(DocCodeCompatabilityViewModel model);
    }
}
