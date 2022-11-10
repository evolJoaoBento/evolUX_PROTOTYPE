using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Repositories;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class RecuperationService : IRecuperationService
    {
        private readonly IRecuperationRepository _recuperationRepository;
        public RecuperationService(IRecuperationRepository recuperationRepository)
        {
            _recuperationRepository = recuperationRepository;
        }

        public async Task<IFlurlResponse> RegistDetailRecover(string StartBarcode, string EndBarcode, string user, DataTable ServiceCompanyList, bool PermissionLevel)
        {
            var response = await _recuperationRepository.RegistDetailRecover(StartBarcode, EndBarcode, user, ServiceCompanyList, PermissionLevel);
            return response;
        }

        public async Task<IFlurlResponse> RegistPartialRecover(string StartBarcode, string EndBarcode, string user, DataTable ServiceCompanyList, bool PermissionLevel)
        {
            var response = await _recuperationRepository.RegistPartialRecover(StartBarcode, EndBarcode, user, ServiceCompanyList, PermissionLevel);
            return response;
        }

        public async Task<IFlurlResponse> RegistTotalRecover(string FileBarcode, string user, DataTable ServiceCompanyList, bool PermissionLevel)
        {
            var response = await _recuperationRepository.RegistTotalRecover(FileBarcode, user, ServiceCompanyList, PermissionLevel);
            return response;
        }
        public async Task<IFlurlResponse> GetPendingRecoveries(int ServiceCompanyID)
        {
            var response = await _recuperationRepository.GetPendingRecoveries(ServiceCompanyID);
            return response;
        }
        public async Task<IFlurlResponse> GetPendingRecoveriesRegistDetail(int ServiceCompanyID)
        {
            var response = await _recuperationRepository.GetPendingRecoveriesRegistDetail(ServiceCompanyID);
            return response;
        }
    }
}
