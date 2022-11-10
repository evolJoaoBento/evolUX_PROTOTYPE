

using SharedModels.Models.Areas.Finishing;
using SharedModels.Models.General;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IRecuperationRepository
    {
        public Task<IEnumerable<Result>> RegistTotalRecover(string fileBarcode, string user, DataTable serviceCompanyList, bool permissionLevel);
        public Task<IEnumerable<Result>> RegistPartialRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel);
        public Task<IEnumerable<Result>> RegistDetailRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel);
        public Task<IEnumerable<PendingRecovery>> GetPendingRecoveries(int ServiceCompanyID);
        public Task<IEnumerable<PendingRecovery>> GetPendingRecoveriesRegistDetail(int ServiceCompanyID);
    }
}
