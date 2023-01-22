using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IConcludedPrintService
    {
        public Task<IFlurlResponse> RegistPrint(string FileBarcode, string user, string ServiceCompanyList);
    }
}
