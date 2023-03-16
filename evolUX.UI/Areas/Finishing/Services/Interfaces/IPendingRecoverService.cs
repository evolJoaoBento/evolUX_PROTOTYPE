using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IPendingRecoverService
    {
        public Task<ServiceCompaniesViewModel> GetServiceCompanies(string ServiceCompanyList);
        public Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int serviceCompanyID, string serviceCompanyCode);
        public Task<Result> RegistPendingRecover(int serviceCompanyID, string serviceCompanyCode, string recoverType, int userid);
    }
}
