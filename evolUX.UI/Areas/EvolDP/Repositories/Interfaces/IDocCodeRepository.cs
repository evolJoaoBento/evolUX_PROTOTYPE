using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using Shared.ViewModels.General;
using System.Data;

namespace evolUX.UI.Areas.EvolDP.Repositories.Interfaces
{
    public interface IDocCodeRepository
    {
        public Task<DocCodeViewModel> GetDocCodeGroup();
        public Task<DocCodeViewModel> GetDocCode(string docLayout, string docType);
        public Task<DocCodeConfigViewModel> GetDocCodeConfig(int docCodeID);
        public Task<DocCodeConfigOptionsViewModel> GetDocCodeConfigOptions(DocCode? docCode);
        public Task<DocCodeConfigViewModel> RegistDocCodeConfig(DocCode docCode);
        public Task<ExceptionLevelViewModel> GetExceptionLevel(int level);
        public Task<DocCodeConfigViewModel> ChangeDocCode(DocCode docCode);
        public Task<ResultsViewModel> DeleteDocCodeConfig(int docCodeID, int startDate);
        public Task<ResultsViewModel> DeleteDocCode(DocCode docCode);
        public Task<DocCodeCompatibilityViewModel> GetCompatibility(int docCodeID);
        public Task<DocCodeCompatibilityViewModel> ChangeCompatibility(int docCodeID, List<string> docCodeList);
    }
}