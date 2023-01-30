using Flurl.Http;
using Shared.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Repositories.Interfaces
{
    public interface IPendingRegistRepository
    {
        public Task<PendingRegistViewModel> GetPendingRegist(string ServiceCompanyList);
        public Task<PendingRegistDetailViewModel> GetPendingRegistDetail(int runID, string ServiceCompanyList);
    }
}