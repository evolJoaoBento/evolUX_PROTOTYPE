﻿using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
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
        public Task<IEnumerable<ExceptionLevel>> SetExceptionLevel(int level, int exceptionID, string exceptionCode, string exceptionDescription);
        public Task<IEnumerable<ExceptionLevel>> DeleteExceptionLevel(int level, int exceptionID);

        public Task<IEnumerable<int>> GetAggregationList();
        public Task<IEnumerable<string>> GetPrintMatchCode();
        public Task<GenericOptionList> GetSuporTypeOptionList();

        public Task<Result> DeleteDocCodeConfig(int docCodeID, int startDate);
        public Task<Result> DeleteDocCode(int docCodeID);

        public Task<IEnumerable<AggregateDocCode>> GetCompatibility(int docCodeID);
        public Task<IEnumerable<AggregateDocCode>> ChangeCompatibility(int docCodeID, DataTable docCodeList);
        public Task<DocCodeData4ScriptViewModel> DocCodeData4Script(int docCodeID, int startDate);
        
    }
}
