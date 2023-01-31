using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using System.Data;
using evolUX.API.Areas.Core.Repositories.Interfaces;

namespace evolUX.API.Areas.Finishing.Services
{
    public class PendingRegistService : IPendingRegistService
    {
        private readonly IWrapperRepository _repository;
        public PendingRegistService(IWrapperRepository repository)
        {
            _repository = repository;
        }



        public async Task<PendingRegistViewModel> GetPendingRegist(DataTable ServiceCompanyList)
        {
            PendingRegistViewModel viewModel = new PendingRegistViewModel();
            viewModel.PendingRegist = await _repository.PendingRegist.GetPendingRegist(ServiceCompanyList);
            if (viewModel.PendingRegist == null)
            {
                throw new NullReferenceException("No registries were found!");
            }
            return viewModel;
        }

        public async Task<PendingRegistDetailViewModel> GetPendingRegistDetail(int RunID, DataTable ServiceCompanyList)
        {
            PendingRegistDetailViewModel viewModel = new PendingRegistDetailViewModel();
            viewModel.PendingRegistDetail = await _repository.PendingRegist.GetPendingRegistDetail(RunID, ServiceCompanyList);
            if (viewModel.PendingRegistDetail == null)
            {
                throw new NullReferenceException("No registries were found!");
            }
            return viewModel;
        }
    }
}
