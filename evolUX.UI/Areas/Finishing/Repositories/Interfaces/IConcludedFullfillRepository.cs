using Flurl.Http;
using Shared.ViewModels.General;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Repositories.Interfaces
{
    public interface IConcludedFulfilmentRepository
    {
        public Task<ResultsViewModel> RegistFullFill(string FileBarcode, string user, string ServiceCompanyList);
    }
}