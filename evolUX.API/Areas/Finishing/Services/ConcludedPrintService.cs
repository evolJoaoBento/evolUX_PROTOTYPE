using Shared.Models.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services
{
    public class ConcludedPrintService : IConcludedPrintService
    {
        private readonly IWrapperRepository _repository;
        public ConcludedPrintService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result>> RegistPrint(string fileBarcode, string user, DataTable serviceCompanyList)
        {
            IEnumerable<Result> results = await _repository.PrintedFiles.RegistPrint(fileBarcode, user,  serviceCompanyList);
            if (results == null)
            {

            }
            return results;
        }
    }
}
