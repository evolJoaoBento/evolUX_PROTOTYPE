using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using System.Data;
using Shared.Models.Areas.evolDP;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IRecoverService
    {
        public Task<Result> RegistTotalRecover(string fileBarcode, string user, DataTable serviceCompanyList, bool permissionLevel);
        public Task<Result> RegistPartialRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel);
        public Task<Result> RegistDetailRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel);
        //public Task<IEnumerable<Company>> GetPendingRecoveries(int ServiceCompanyID);
        //public Task<PendingRecoverDetailInfo> GetPendingRecoveriesRegistDetail(int ServiceCompanyID);

    }
}
