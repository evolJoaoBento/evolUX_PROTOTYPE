using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using evolUX.API.Data.Interfaces;
using System.Data;
using Shared.Models.Areas.evolDP;
using Shared.Models.Areas.Finishing;

namespace evolUX.API.Areas.Finishing.Services
{
    public class PendingRecoverService : IPendingRecoverService
    {
        private readonly IWrapperRepository _repository;
        public PendingRecoverService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Company>> GetServiceCompanies(DataTable ServiceCompanyList)
        {
            IEnumerable<Company> serviceCompanies = await _repository.PendingRecover.GetServiceCompanies(ServiceCompanyList);
            if (serviceCompanies == null)
            {

            }

            return serviceCompanies;
        }
        
        public async Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int serviceCompanyID)
        {
            PendingRecoverDetailViewModel viewmodel = new PendingRecoverDetailViewModel();

            viewmodel.PendingRecoverDetail.PendingRecoverFiles = (List<PendingRecoverElement>)await _repository.PendingRecover.GetPendingRecoverFiles(serviceCompanyID);
            viewmodel.PendingRecoverDetail.PendingRecoverRegistDetailFiles = (List<PendingRecoverElement>)await _repository.PendingRecover.GetPendingRecoverRegistDetailFiles(serviceCompanyID);

            return viewmodel;
        }


    }
}
