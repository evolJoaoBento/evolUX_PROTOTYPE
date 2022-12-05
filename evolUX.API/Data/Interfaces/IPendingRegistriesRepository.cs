

using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IPendingRegistriesRepository
    {
        public Task<IEnumerable<PendingRegistry>> GetPendingRegistries(DataTable serviceCompanyList);
    }
}
