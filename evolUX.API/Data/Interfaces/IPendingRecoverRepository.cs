

using Shared.Models.Areas.evolDP;
using Shared.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IPendingRecoverRepository
    {
        public Task<IEnumerable<Company>> GetServiceCompanies(DataTable ServiceCompanyList);
        public Task<IEnumerable<PendingRecoverDetailInfo>> GetPendingRecoverFiles(int serviceCompanyID);
        public Task<IEnumerable<PendingRecoverDetailInfo>> GetPendingRecoverRegistDetailFiles(int serviceCompanyID);
    }
}
