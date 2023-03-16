using Shared.Models.General;
using Shared.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IConcludedPrintingService
    {
        public Task<Result> RegistPrint(string fileBarcode, string user, DataTable serviceCompanyList);
    }
}
