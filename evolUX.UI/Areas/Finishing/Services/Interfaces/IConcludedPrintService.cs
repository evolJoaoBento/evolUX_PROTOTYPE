using Flurl.Http;
using Shared.ViewModels.General;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IConcludedPrintService
    {
        public Task<ResultsViewModel> RegistPrint(string FileBarcode, string user, string ServiceCompanyList);
    }
}
