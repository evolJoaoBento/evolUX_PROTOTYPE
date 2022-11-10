using Flurl.Http;
using System.Data;

namespace evolUX.UI.Repositories
{
    public interface IConcludedPrintRepository
    {
        public Task<IFlurlResponse> RegistPrint(string FileBarcode, string user, DataTable ServiceCompanyList);
    }
}