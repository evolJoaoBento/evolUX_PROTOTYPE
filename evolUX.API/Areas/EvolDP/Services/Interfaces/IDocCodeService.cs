using Shared.ViewModels.Areas.evolDP;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using System.Data;

namespace evolUX.API.Areas.EvolDP.Services.Interfaces
{
    public interface IDocCodeService
    {
        public Task<DocCodeViewModel> GetDocCodeGroup();
        public Task<DocCodeViewModel> GetDocCode(string docLayout, string docType);
        public Task<DocCodeConfigViewModel> GetDocCodeConfig(int docCodeID, DateTime? startDate, bool? maxDateFlag);
        public Task<DocCodeConfigOptionsViewModel> GetDocCodeConfigOptions(DocCode? docCode);
        public Task<ExceptionLevelViewModel> GetExceptionLevel(int level);
        public Task<ExceptionLevelViewModel> SetExceptionLevel(int level, int exceptionID, string exceptionCode, string exceptionDescription);
        public Task<ExceptionLevelViewModel> DeleteExceptionLevel(int level, int exceptionID);
        public Task<DocCodeViewModel> SetDocCodeConfig(DocCode docCode);
        public Task<DocCodeViewModel> ChangeDocCode(DocCode docCode);
        public Task<Result> DeleteDocCodeConfig(int docCodeID, int startDate);
        public Task<Result> DeleteDocCode(int docCodeID);

        public Task<IEnumerable<AggregateDocCode>> GetCompatibility(int docCodeID);
        public Task<IEnumerable<AggregateDocCode>> ChangeCompatibility(int docCodeID, DataTable docCodeList);
        public Task<DocCodeData4ScriptViewModel> DocCodeData4Script(int docCodeID, int startDate);

    }
}
