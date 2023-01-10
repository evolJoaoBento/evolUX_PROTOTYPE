using Flurl.Http;
using System.Data;

namespace evolUX.UI.Repositories
{
    public interface IConcludedFullfillRepository
    {
        public Task<IFlurlResponse> RegistFullFill(string FileBarcode, string user, DataTable ServiceCompanyList);
    }
}