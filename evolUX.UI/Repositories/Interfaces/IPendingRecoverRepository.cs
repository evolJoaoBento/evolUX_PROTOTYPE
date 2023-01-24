using Flurl.Http;
using Shared.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.UI.Repositories.Interfaces
{
    public interface IPendingRecoverRepository
    {
        public Task<ServiceCompanyViewModel> GetServiceCompanies(string ServiceCompanyList);
        public Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int ServiceCompanyID);
    }
}