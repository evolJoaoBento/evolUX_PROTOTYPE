using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IConcludedFullfillService
    {
        public Task<IFlurlResponse> RegistFullFill(string FileBarcode, string user, string ServiceCompanyList);
    }
}
