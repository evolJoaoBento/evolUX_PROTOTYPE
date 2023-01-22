using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services
{
    public class RecoverService : IRecoverService
    {
        private readonly IWrapperRepository _repository;
        public RecoverService(IWrapperRepository repository)
        {
            _repository = repository;
        }

       

        public async Task<Result> RegistDetailRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel)
        {
            Result results = await _repository.Recover.RegistDetailRecover(startBarcode, endBarcode, user, serviceCompanyList, permissionLevel);
            if (results == null)
            {

            }
            return results;
        }

        public async Task<Result> RegistPartialRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel)
        {
            Result results = await _repository.Recover.RegistDetailRecover(startBarcode, endBarcode, user, serviceCompanyList, permissionLevel);
            if (results == null)
            {

            }
            return results;
        }

        public async Task<Result> RegistTotalRecover(string fileBarcode, string user, DataTable serviceCompanyList, bool permissionLevel)
        {
            Result results = await _repository.Recover.RegistTotalRecover(fileBarcode, user,  serviceCompanyList, permissionLevel);
            if (results == null)
            {

            }
            return results;
        }

        public async Task<IEnumerable<PendingRecovery>> GetPendingRecoveries(int ServiceCompanyID)
        {
            IEnumerable<PendingRecovery> pendingRecoveries = await _repository.Recover.GetPendingRecoveries(ServiceCompanyID);
            if (pendingRecoveries == null)
            {

            }
            return pendingRecoveries;
        }
        public async Task<IEnumerable<PendingRecovery>> GetPendingRecoveriesRegistDetail(int ServiceCompanyID)
        {
            IEnumerable<PendingRecovery> pendingRecoveries = await _repository.Recover.GetPendingRecoveries(ServiceCompanyID);
            if (pendingRecoveries == null)
            {

            }
            return pendingRecoveries;
        }
    }
}
