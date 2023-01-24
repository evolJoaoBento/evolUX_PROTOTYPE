using Shared.ViewModels.Areas.Finishing;
using System.Data;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IPendingRecoverService
    {
        public Task<IEnumerable<Company>> GetServiceCompanies(DataTable ServiceCompanyList);
        public Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int serviceCompanyID);
        public Task<Result> RegistPendingRecover(int serviceCompanyID, string serviceCompanyCode, string recoverType, int userID);
    }
}
