using SharedModels.Models.Areas.Finishing;
using SharedModels.Models.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using SharedModels.ViewModels.Areas.Finishing;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services
{
    public class RecuperationService : IRecuperationService
    {
        private readonly IWrapperRepository _repository;
        public RecuperationService(IWrapperRepository repository)
        {
            _repository = repository;
        }

       

        public async Task<IEnumerable<Result>> RegistDetailRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel)
        {
            IEnumerable<Result> results = await _repository.Recuperation.RegistDetailRecover(startBarcode, endBarcode, user, serviceCompanyList, permissionLevel);
            if (results == null)
            {

            }
            return results;
        }

        public async Task<IEnumerable<Result>> RegistPartialRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel)
        {
            IEnumerable<Result> results = await _repository.Recuperation.RegistDetailRecover(startBarcode, endBarcode, user, serviceCompanyList, permissionLevel);
            if (results == null)
            {

            }
            return results;
        }

        public async Task<IEnumerable<Result>> RegistTotalRecover(string fileBarcode, string user, DataTable serviceCompanyList, bool permissionLevel)
        {
            IEnumerable<Result> results = await _repository.Recuperation.RegistTotalRecover(fileBarcode, user,  serviceCompanyList, permissionLevel);
            if (results == null)
            {

            }
            return results;
        }

        public async Task<IEnumerable<PendingRecovery>> GetPendingRecoveries(int ServiceCompanyID)
        {
            IEnumerable<PendingRecovery> pendingRecoveries = await _repository.Recuperation.GetPendingRecoveries(ServiceCompanyID);
            if (pendingRecoveries == null)
            {

            }
            return pendingRecoveries;
        }
        public async Task<IEnumerable<PendingRecovery>> GetPendingRecoveriesRegistDetail(int ServiceCompanyID)
        {
            IEnumerable<PendingRecovery> pendingRecoveries = await _repository.Recuperation.GetPendingRecoveries(ServiceCompanyID);
            if (pendingRecoveries == null)
            {

            }
            return pendingRecoveries;
        }
    }
}
