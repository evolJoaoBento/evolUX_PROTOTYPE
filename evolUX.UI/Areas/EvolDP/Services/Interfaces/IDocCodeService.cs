using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using Shared.ViewModels.General;
using System;
using System.Data;

namespace evolUX.UI.Areas.EvolDP.Services.Interfaces
{
    public interface IDocCodeService
    {
        public Task<DocCodeViewModel> GetDocCodeGroup();
        public Task<DocCodeViewModel> GetDocCode(string docLayout, string docType);
        public Task<DocCodeConfigViewModel> GetDocCodeConfig(int docCodeID);
        public Task<DocCodeConfigOptionsViewModel> GetDocCodeConfigOptions(DocCode? docCode);
        public Task<DocCodeConfigViewModel> RegistDocCodeConfig(DocCode docCode);
        public Task<ExceptionLevelViewModel> GetExceptionLevel(int level);
        public Task<ExceptionLevelViewModel> SetExceptionLevel(int level, int exceptionID, string exceptionCode, string exceptionDescription);
        public Task<ExceptionLevelViewModel> DeleteExceptionLevel(int level, int exceptionID);

        public Task<DocCodeConfigViewModel> ChangeDocCode(DocCode docCode);
        public Task<ResultsViewModel> DeleteDocCodeConfig(int docCodeID, int startDate);
        public Task<ResultsViewModel> DeleteDocCode(DocCode docCode);
        public Task<DocCodeCompatibilityViewModel> GetCompatibility(int docCodeID);
        public Task<DocCodeCompatibilityViewModel> ChangeCompatibility(int docCodeID, List<string> docCodeList);

        public Task<List<string>> DocCodeData4Script(int docCodeID, int startDate);
    }
}
