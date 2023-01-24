using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IPendingRecoverService
    {
        public Task<ServiceCompanyViewModel> GetServiceCompanies(string ServiceCompanyList);
        public Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int serviceCompanyID);
    }
}
