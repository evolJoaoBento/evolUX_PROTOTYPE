using Flurl.Http;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using Shared.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Repositories.Interfaces
{
    public interface IPendingRecoverRepository
    {
        public Task<ServiceCompaniesViewModel> GetServiceCompanies(string ServiceCompanyList);
        public Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int ServiceCompanyID, string serviceCompanyCode);
        public Task<Result> RegistPendingRecover(int serviceCompanyID, string serviceCompanyCode, string recoverType, int userid);
    }
}