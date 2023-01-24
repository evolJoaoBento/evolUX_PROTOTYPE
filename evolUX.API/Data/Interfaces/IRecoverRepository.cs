

using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IRecoverRepository
    {
        public Task<Result> RegistTotalRecover(string fileBarcode, string user, DataTable serviceCompanyList, bool permissionLevel);
        public Task<Result> RegistPartialRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel);
        public Task<Result> RegistDetailRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel);
        //public Task<IEnumerable<PendingRecoverElement>> GetPendingRecoveries(int ServiceCompanyID);
        //public Task<IEnumerable<PendingRecoverElement>> GetPendingRecoveriesRegistDetail(int ServiceCompanyID);
    }
}
