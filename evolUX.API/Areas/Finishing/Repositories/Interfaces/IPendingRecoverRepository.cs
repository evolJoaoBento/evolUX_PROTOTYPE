using Shared.Models.Areas.evolDP;
using Shared.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Finishing.Repositories.Interfaces
{
    public interface IPendingRecoverRepository
    {
        public Task<IEnumerable<PendingRecoverElement>> GetPendingRecoverFiles(int serviceCompanyID);
        public Task<IEnumerable<PendingRecoverElement>> GetPendingRecoverRegistDetailFiles(int serviceCompanyID);
    }
}
