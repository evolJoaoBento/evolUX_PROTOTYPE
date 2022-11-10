

using SharedModels.Models.General;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IConcludedEnvelopeRepository
    {
        public Task<IEnumerable<Result>> RegistFullFill(string fileBarcode, string user, DataTable serviceCompanyList);
    }
}
