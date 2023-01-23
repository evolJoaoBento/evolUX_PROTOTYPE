using Flurl.Http;
using Shared.ViewModels.General;
using System.Data;

namespace evolUX.UI.Repositories.Interfaces
{
    public interface IConcludedPrintRepository
    {
        public Task<ResultsViewModel> RegistPrint(string FileBarcode, string user, string ServiceCompanyList);
    }
}