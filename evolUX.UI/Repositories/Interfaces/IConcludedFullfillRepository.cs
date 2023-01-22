using Flurl.Http;
using System.Data;

namespace evolUX.UI.Repositories.Interfaces
{
    public interface IConcludedFullfillRepository
    {
        public Task<IFlurlResponse> RegistFullFill(string FileBarcode, string user, string ServiceCompanyList);
    }
}