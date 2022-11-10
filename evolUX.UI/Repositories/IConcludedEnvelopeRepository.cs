using Flurl.Http;
using System.Data;

namespace evolUX.UI.Repositories
{
    public interface IConcludedEnvelopeRepository
    {
        public Task<IFlurlResponse> RegistFullFill(string FileBarcode, string user, DataTable ServiceCompanyList);
    }
}