using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IPendingRegistriesService
    {
        public Task<PendingRegistriesViewModel> GetPendingRegistries(DataTable ServiceCompanyList);
    }
}
