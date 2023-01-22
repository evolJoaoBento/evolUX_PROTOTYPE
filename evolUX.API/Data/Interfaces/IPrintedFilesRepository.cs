using Shared.Models.General;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IPrintedFilesRepository
    {
        public Task<Result> RegistPrint(string fileBarcode, string user, DataTable serviceCompanyList);
    }
}
