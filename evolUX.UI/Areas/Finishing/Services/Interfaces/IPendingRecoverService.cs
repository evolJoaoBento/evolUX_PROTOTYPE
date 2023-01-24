using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;
using Shared.Models.General;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IPendingRecoverService
    {
        public Task<ServiceCompanyViewModel> GetServiceCompanies(string ServiceCompanyList);
        public Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int serviceCompanyID, string serviceCompanyCode);
        public Task<Result> RegistPendingRecover(int serviceCompanyID, string serviceCompanyCode, string recoverType, int userid);
    }
}
