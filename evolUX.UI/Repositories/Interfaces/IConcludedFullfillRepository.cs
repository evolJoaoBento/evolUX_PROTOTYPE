using Flurl.Http;
using Shared.ViewModels.General;
using System.Data;

namespace evolUX.UI.Repositories.Interfaces
{
    public interface IConcludedFullfillRepository
    {
        public Task<ResultsViewModel> RegistFullFill(string FileBarcode, string user, string ServiceCompanyList);
    }
}