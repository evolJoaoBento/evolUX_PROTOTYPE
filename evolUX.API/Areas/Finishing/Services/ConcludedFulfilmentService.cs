using Shared.Models.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.General;
using System.Data;
using evolUX.API.Areas.Core.Repositories.Interfaces;

namespace evolUX.API.Areas.Finishing.Services
{
    public class ConcludedFulfilmentService : IConcludedFulfilmentService
    {
        private readonly IWrapperRepository _repository;
        public ConcludedFulfilmentService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> RegistFullFill(string fileBarcode, string user, DataTable serviceCompanyList)
        {
            Result results = await _repository.FullfilledFiles.RegistFulFilment(fileBarcode, user,  serviceCompanyList);
            if (results == null)
            {

            }
            return results;
        }
    }
}
