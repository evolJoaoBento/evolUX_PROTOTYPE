using Flurl.Http;
using Shared.ViewModels.General;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Repositories.Interfaces
{
    public interface IRecoverRepository
    {
        public Task<ResultsViewModel> RegistTotalRecover(string FileBarcode, string user, string ServiceCompanyList, bool PermissionLevel);
        public Task<ResultsViewModel> RegistPartialRecover(string StartBarcode, string EndBarcode, string user, string ServiceCompanyList, bool PermissionLevel);
        public Task<ResultsViewModel> RegistDetailRecover(string StartBarcode, string EndBarcode, string user, string ServiceCompanyList, bool PermissionLevel);
        //public Task<IFlurlResponse> GetPendingRecoveries(int ServiceCompanyID);
        //public Task<IFlurlResponse> GetPendingRecoveriesRegistDetail(int ServiceCompanyID);
    }
}