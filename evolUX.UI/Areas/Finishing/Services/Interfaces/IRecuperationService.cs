using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IRecuperationService
    {
        public Task<IFlurlResponse> RegistTotalRecover(string FileBarcode,  string user,  DataTable ServiceCompanyList,  bool PermissionLevel);
        public Task<IFlurlResponse> RegistPartialRecover(string StartBarcode,  string EndBarcode,  string user,  DataTable ServiceCompanyList,  bool PermissionLevel);
        public Task<IFlurlResponse> RegistDetailRecover(string StartBarcode,  string EndBarcode,  string user,  DataTable ServiceCompanyList,  bool PermissionLevel);
        public Task<IFlurlResponse> GetPendingRecoveries(int ServiceCompanyID);
        public Task<IFlurlResponse> GetPendingRecoveriesRegistDetail(int ServiceCompanyID);
    }
}

