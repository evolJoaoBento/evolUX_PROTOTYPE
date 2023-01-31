using Shared.Models.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using System.Data;
using evolUX.API.Areas.Core.Repositories.Interfaces;

namespace evolUX.API.Areas.Finishing.Services
{
    public class ConcludedPrintService : IConcludedPrintService
    {
        private readonly IWrapperRepository _repository;
        public ConcludedPrintService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> RegistPrint(string fileBarcode, string user, DataTable serviceCompanyList)
        {
            Result results = await _repository.PrintedFiles.RegistPrint(fileBarcode, user,  serviceCompanyList);
            if (results == null)
            {

            }
            return results;
        }
    }
}
