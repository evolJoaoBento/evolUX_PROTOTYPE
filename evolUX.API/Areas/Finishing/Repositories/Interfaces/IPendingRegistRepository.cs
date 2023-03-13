using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using System.Data;

namespace evolUX.API.Areas.Finishing.Repositories.Interfaces
{
    public interface IPendingRegistRepository
    {
        public Task<IEnumerable<PendingRegistInfo>> GetPendingRegist(DataTable serviceCompanyList);
        public Task<PendingRegistDetailInfo> GetPendingRegistDetail(int RunID, DataTable serviceCompanyList);
    }
}
