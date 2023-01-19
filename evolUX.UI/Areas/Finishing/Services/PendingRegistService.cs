using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;
using evolUX.UI.Repositories.Interfaces;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class PendingRegistService : IPendingRegistService
    {
        private readonly IPendingRegistRepository _pendingRegistRepository;
        public PendingRegistService(IPendingRegistRepository pendingRegistRepository)
        {
            _pendingRegistRepository = pendingRegistRepository;
        }
        public async Task<PendingRegistViewModel> GetPendingRegist(string ServiceCompanyList)
        {
            var response = await _pendingRegistRepository.GetPendingRegist(ServiceCompanyList);
            return response;
        }
        public async Task<PendingRegistDetailViewModel> GetPendingRegistDetail(int runID, string ServiceCompanyList)
        {
            var response = await _pendingRegistRepository.GetPendingRegistDetail(runID, ServiceCompanyList);
            return response;
        }
    }
}
