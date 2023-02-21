using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.API.Areas.EvolDP.Repositories.Interfaces
{
    public interface IDocCodeRepository
    {
        public Task<IEnumerable<DocCode>> GetDocCodeGroup();
        public Task<IEnumerable<DocCode>> GetDocCode(int docCodeID);
        public Task<IEnumerable<DocCode>> GetDocCode(string docLayout, string docType, int numRows);
        public Task<IEnumerable<DocCodeConfig>> GetDocCodeConfig(int docCodeID, int? startDate, bool? maxDateFlag);
        public Task<IEnumerable<DocCodeConfig>> GetDocCodeConfig(int docCodeID, DateTime? startDate, bool? maxDateFlag);
        public Task<IEnumerable<ExceptionLevel>> GetDocExceptionsLevel(int level);
        public Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMediaGroups(int? envMediaGroupID);
        public Task<IEnumerable<int>> GetAggregationList();
        public Task<IEnumerable<ExpeditionsType>> GetExpeditionTypes(int? expeditionType);
        public Task<IEnumerable<ExpCompanyServiceTask>> GetExpCompanyServiceTask(string expCode);
        public Task<IEnumerable<ServiceTask>> GetServiceTasks(int? serviceTaskID);
        public Task<GenericOptionList> GetSuporTypeOptionList();
        //public Task<IEnumerable<GenericOptionValue>> GetOptionList(string option);
        public Task<IEnumerable<DocCode>> PostDocCodeConfig(DocCode docCode);
        public Task<IEnumerable<string>> DeleteDocCode(int docCodeID);
        Task<IEnumerable<AggregateDocCode>> GetAggregateDocCodes(int docCodeID);
        Task<AggregateDocCode> GetAggregateDocCode(int docCodeID);
        Task ChangeCompatibility(DocCodeCompatabilityViewModel model);
    }
}
