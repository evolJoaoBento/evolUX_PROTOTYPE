using Flurl.Http;
using Shared.ViewModels.General;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IConcludedFullfillService
    {
        public Task<ResultsViewModel> RegistFullFill(string FileBarcode, string user, string ServiceCompanyList);
    }
}
