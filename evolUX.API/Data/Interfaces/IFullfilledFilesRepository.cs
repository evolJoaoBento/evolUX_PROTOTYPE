using Shared.Models.General;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IFullfilledFilesRepository
    {
        public Task<Result> RegistFullFill(string fileBarcode, string user, DataTable serviceCompanyList);
    }
}
