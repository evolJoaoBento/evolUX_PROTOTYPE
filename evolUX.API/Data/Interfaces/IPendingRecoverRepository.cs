

using Shared.Models.Areas.evolDP;
using Shared.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IPendingRecoverRepository
    {
        public Task<IEnumerable<Company>> GetServiceCompanies(DataTable ServiceCompanyList);
        public Task<IEnumerable<PendingRecoverElement>> GetPendingRecoverFiles(int serviceCompanyID);
        public Task<IEnumerable<PendingRecoverElement>> GetPendingRecoverRegistDetailFiles(int serviceCompanyID);
    }
}
