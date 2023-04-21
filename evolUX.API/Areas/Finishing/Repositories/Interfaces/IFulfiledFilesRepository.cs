using Shared.Models.General;
using System.Data;

namespace evolUX.API.Areas.Finishing.Repositories.Interfaces
{
    public interface IFulfiledFilesRepository
    {
        public Task<Result> RegistFulFilment(string fileBarcode, string user, DataTable serviceCompanyList);
    }
}
