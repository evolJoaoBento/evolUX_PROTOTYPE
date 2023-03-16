using Shared.ViewModels.Areas.Finishing;
using Shared.Models.General;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IConcludedFulfilmentService
    {
        public Task<Result> RegistFullFill(string fileBarcode, string user, DataTable serviceCompanyList);
    }
}
