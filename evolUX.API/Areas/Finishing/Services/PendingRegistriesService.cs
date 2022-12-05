using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services
{
    public class PendingRegistriesService : IPendingRegistriesService
    {
        private readonly IWrapperRepository _repository;
        public PendingRegistriesService(IWrapperRepository repository)
        {
            _repository = repository;
        }



        public async Task<PendingRegistriesViewModel> GetPendingRegistries(DataTable ServiceCompanyList)
        {
            PendingRegistriesViewModel viewModel = new PendingRegistriesViewModel();
            viewModel.PendingRegistries = await _repository.PendingRegistries.GetPendingRegistries(ServiceCompanyList);
            if (viewModel.PendingRegistries == null)
            {
                throw new NullReferenceException("No registries were found!");
            }
            return viewModel;
        }
    }
}
