using Shared.Models.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.General;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services
{
    public class ConcludedFullfillService : IConcludedFullfillService
    {
        private readonly IWrapperRepository _repository;
        public ConcludedFullfillService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> RegistFullFill(string fileBarcode, string user, DataTable serviceCompanyList)
        {
            Result results = await _repository.FullfilledFiles.RegistFullFill(fileBarcode, user,  serviceCompanyList);
            if (results == null)
            {

            }
            return results;
        }
    }
}
