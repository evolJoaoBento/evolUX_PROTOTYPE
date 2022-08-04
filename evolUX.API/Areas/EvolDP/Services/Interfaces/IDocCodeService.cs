using evolUX.API.Areas.EvolDP.Models;

namespace evolUX.API.Areas.EvolDP.Services.Interfaces
{
    public interface IDocCodeService
    {
        public Task<IEnumerable<DocCode>> GetDocCodeGroup();
        public Task<IEnumerable<DocCode>> GetDocCode(string docLayout, string docType);
        public Task<IEnumerable<DocCodeConfig>> GetDocCodeConfig(string ID);
        public Task<DocCodeConfig> GetDocCodeConfig(string ID, int startdate);
        public Task<DocCodeConfig> GetDocCodeConfigOptions(string ID);
        public Task<IEnumerable<DocException>> GetDocExceptionsLevel1();
        public Task<IEnumerable<DocException>> GetDocExceptionsLevel2();
        public Task<IEnumerable<DocException>> GetDocExceptionsLevel3();
        public Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMediaGroups(string envMediaGroupID);
        public Task<IEnumerable<int>> GetAggregationList(string aggrCompatibility);
        public Task<IEnumerable<Company>> GetExpeditionCompanies(string companyName);
        public Task<IEnumerable<ExpeditionsType>> GetExpeditionTypes(string expeditionType);
        public Task<IEnumerable<TreatmentType>> GetTreatmentTypes(string treatmentType);
        public Task<IEnumerable<int>> GetFinishingList(string finishing);
        public Task<IEnumerable<int>> GetArchiveList(string archive);
        public Task<IEnumerable<Email>> GetEmailList(string email);
        public Task<IEnumerable<int>> GetEmailHideList(string emailHide);
        public Task<IEnumerable<Electronic>> GetElectronicList(string electronic);
        public Task<IEnumerable<int>> GetElectronicHideList(string electronicHide);
    }
}
