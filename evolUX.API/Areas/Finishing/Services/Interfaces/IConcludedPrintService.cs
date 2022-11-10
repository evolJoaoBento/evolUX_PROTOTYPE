using SharedModels.Models.General;
using SharedModels.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IConcludedPrintService
    {
        public Task<IEnumerable<Result>> RegistPrint(string fileBarcode, string user, DataTable serviceCompanyList);
    }
}
