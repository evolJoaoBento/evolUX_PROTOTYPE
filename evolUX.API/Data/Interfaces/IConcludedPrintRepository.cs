

using SharedModels.Models.General;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IConcludedPrintRepository
    {
        public Task<IEnumerable<Result>> RegistPrint(string fileBarcode, string user, DataTable serviceCompanyList);
    }
}
