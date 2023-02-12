using Shared.ViewModels.Areas.evolDP;
using Shared.Models.Areas.evolDP;

namespace evolUX.API.Areas.EvolDP.Services.Interfaces
{
    public interface IDocCodeService
    {
        public Task<DocCodeViewModel> GetDocCodeGroup();
        public Task<DocCodeViewModel> GetDocCode(string docLayout, string docType);
        public Task<DocCodeViewModel> GetDocCodeConfig(int docCodeID, DateTime? startDate, bool? maxDateFlag);
        public Task<DocCodeConfigOptionsViewModel> GetDocCodeConfigOptions(DocCode? docCode);
        public Task PostDocCodeConfig(DocCode docCode);
        public Task<IEnumerable<string>> DeleteDocCode(int docCodeID);
        public Task ChangeCompatibility(DocCodeCompatabilityViewModel model);
        public Task<IEnumerable<AggregateDocCode>> GetAggregateDocCodes(int docCodeID);
        public Task<AggregateDocCode> GetAggregateDocCode(int docCodeID);
    }
}
