using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.EvolDP.Repositories.Interfaces
{
    public interface IDocCodeRepository
    {
        public Task<IEnumerable<DocCode>> GetDocCodeGroup();
        public Task<IEnumerable<DocCode>> GetDocCode(int docCodeID);
        public Task<IEnumerable<DocCode>> GetDocCode(string docLayout, string docType, int numRows);
        public Task<IEnumerable<DocCodeConfig>> GetDocCodeConfig(int docCodeID, int? startDate, bool? maxDateFlag);
        public Task<IEnumerable<DocCodeConfig>> GetDocCodeConfig(int docCodeID, DateTime? startDate, bool? maxDateFlag);
        public Task<IEnumerable<DocCode>> SetDocCodeConfig(DocCode docCode);
        public Task<IEnumerable<DocCode>> ChangeDocCode(DocCode docCode);

        public Task<IEnumerable<ExceptionLevel>> GetExceptionLevel(int level);
        public Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMediaGroups(int? envMediaGroupID);
        public Task<IEnumerable<int>> GetAggregationList();
        public Task<IEnumerable<ExpeditionsType>> GetExpeditionTypes(int? expeditionType);
        public Task<IEnumerable<ExpCompanyServiceTask>> GetExpCompanyServiceTask(string expCode);
        public Task<IEnumerable<ServiceTask>> GetServiceTasks(int? serviceTaskID);
        public Task<GenericOptionList> GetSuporTypeOptionList();

        public Task<Result> DeleteDocCodeConfig(int docCodeID, int startDate);
        public Task<Result> DeleteDocCode(int docCodeID);

        public Task<IEnumerable<AggregateDocCode>> GetCompatibility(int docCodeID);
        public Task<IEnumerable<AggregateDocCode>> ChangeCompatibility(int docCodeID, DataTable docCodeList);
    }
}
