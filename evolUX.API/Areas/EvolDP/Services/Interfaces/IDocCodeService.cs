using Shared.ViewModels.Areas.evolDP;
using Shared.Models.Areas.evolDP;

namespace evolUX.API.Areas.EvolDP.Services.Interfaces
{
    public interface IDocCodeService
    {
        public Task<DocCodeViewModel> GetDocCodeGroup();
        public Task<DocCodeViewModel> GetDocCode(string docLayout, string docType);
        public Task<DocCodeConfigViewModel> GetDocCodeConfig(int docCodeID);
        public Task<DocCodeConfig> GetDocCodeConfig(int docCodeID, int startdate);
        public Task<DocCodeConfig> GetDocCodeConfigOptions(int docCodeID);
        public Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel1();
        public Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel2();
        public Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel3();
        public Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMediaGroups(string envMediaGroupID);
        public Task<IEnumerable<int>> GetAggregationList(string aggrCompatibility);
        public Task<IEnumerable<Company>> GetExpeditionCompanies(string companyName);
        public Task<IEnumerable<ExpeditionsType>> GetExpeditionTypes(string expeditionType);
        public Task<IEnumerable<ServiceTask>> GetServiceTasks(string serviceTask);
        public Task<IEnumerable<int>> GetFinishingList(string finishing);
        public Task<IEnumerable<int>> GetArchiveList(string archive);
        public Task<IEnumerable<Email>> GetEmailList(string email);
        public Task<IEnumerable<int>> GetEmailHideList(string emailHide);
        public Task<IEnumerable<Electronic>> GetElectronicList(string electronic);
        public Task<IEnumerable<int>> GetElectronicHideList(string electronicHide);
        public Task PostDocCodeConfig(DocCodeConfig model);
        public Task<IEnumerable<string>> DeleteDocCode(int docCodeID);
        public Task ChangeCompatibility(DocCodeCompatabilityViewModel model);
        public Task<IEnumerable<AggregateDocCode>> GetAggregateDocCodes(int docCodeID);
        public Task<AggregateDocCode> GetAggregateDocCode(int docCodeID);
    }
}
