using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IPendingRegistService
    {
        public Task<PendingRegistViewModel> GetPendingRegist(string ServiceCompanyList);
        public Task<PendingRegistDetailViewModel> GetPendingRegistDetail(int runID, string ServiceCompanyList);
    }
}
