using Shared.ViewModels.Areas.Finishing;
using System.Data;
using Shared.Models.Areas.evolDP;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IPendingRecoverService
    {
        public Task<IEnumerable<Company>> GetServiceCompanies(DataTable ServiceCompanyList);
        public Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int serviceCompanyID);
    }
}
