using SharedModels.ViewModels.Areas.Finishing;
using SharedModels.Models.General;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IConcludedEnvelopeService
    {
        public Task<IEnumerable<Result>> RegistFullFill(string fileBarcode, string user, DataTable serviceCompanyList);
    }
}
