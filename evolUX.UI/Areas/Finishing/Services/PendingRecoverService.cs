using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;
using evolUX.UI.Repositories.Interfaces;
using Shared.Models.General;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class PendingRecoverService : IPendingRecoverService
    {
        private readonly IPendingRecoverRepository _pendingRecoverRepository;
        public PendingRecoverService(IPendingRecoverRepository pendingRecoverRepository)
        {
            _pendingRecoverRepository = pendingRecoverRepository;
        }
        public async Task<ServiceCompanyViewModel> GetServiceCompanies(string ServiceCompanyList)
        {
            var response = await _pendingRecoverRepository.GetServiceCompanies(ServiceCompanyList);
            return response;
        }
        public async Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int serviceCompanyID)
        {
            var response = await _pendingRecoverRepository.GetPendingRecoveries(serviceCompanyID);
            return response;
        }
        public async Task<Result> RegistPendingRecover(int serviceCompanyID, string serviceCompanyCode, string recoverType, int userid)
        {
            var response = await _pendingRecoverRepository.RegistPendingRecover(serviceCompanyID, serviceCompanyCode, recoverType, userid);
            return response;
        }
    }
}
