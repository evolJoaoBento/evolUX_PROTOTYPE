using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Repositories;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class RecoverService : IRecoverService
    {
        private readonly IRecoverRepository _recoverRepository;
        public RecoverService(IRecoverRepository recoverRepository)
        {
            _recoverRepository = recoverRepository;
        }

        public async Task<IFlurlResponse> RegistDetailRecover(string StartBarcode, string EndBarcode, string user, DataTable ServiceCompanyList, bool PermissionLevel)
        {
            var response = await _recoverRepository.RegistDetailRecover(StartBarcode, EndBarcode, user, ServiceCompanyList, PermissionLevel);
            return response;
        }

        public async Task<IFlurlResponse> RegistPartialRecover(string StartBarcode, string EndBarcode, string user, DataTable ServiceCompanyList, bool PermissionLevel)
        {
            var response = await _recoverRepository.RegistPartialRecover(StartBarcode, EndBarcode, user, ServiceCompanyList, PermissionLevel);
            return response;
        }

        public async Task<IFlurlResponse> RegistTotalRecover(string FileBarcode, string user, DataTable ServiceCompanyList, bool PermissionLevel)
        {
            var response = await _recoverRepository.RegistTotalRecover(FileBarcode, user, ServiceCompanyList, PermissionLevel);
            return response;
        }
        public async Task<IFlurlResponse> GetPendingRecoveries(int ServiceCompanyID)
        {
            var response = await _recoverRepository.GetPendingRecoveries(ServiceCompanyID);
            return response;
        }
        public async Task<IFlurlResponse> GetPendingRecoveriesRegistDetail(int ServiceCompanyID)
        {
            var response = await _recoverRepository.GetPendingRecoveriesRegistDetail(ServiceCompanyID);
            return response;
        }
    }
}
