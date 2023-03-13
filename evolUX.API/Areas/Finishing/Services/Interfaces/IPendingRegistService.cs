using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IPendingRegistService
    {
        public Task<PendingRegistViewModel> GetPendingRegist(DataTable ServiceCompanyList);
        public Task<PendingRegistDetailViewModel> GetPendingRegistDetail(int RunID, DataTable ServiceCompanyList);
    }
}
