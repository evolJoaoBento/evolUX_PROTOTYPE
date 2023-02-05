using Shared.ViewModels.Areas.Finishing;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using Shared.Models.General;
using evolUX.UI.Repositories;

namespace evolUX.UI.Areas.Finishing.Repositories
{
    public class PendingRecoverRepository : RepositoryBase, IPendingRecoverRepository
    {
        public PendingRecoverRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<ServiceCompanyViewModel> GetServiceCompanies(string ServiceCompanyList)
        {
            var response = await _flurlClient.Request("/API/finishing/PendingRecover/GetServiceCompanies")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, ServiceCompanyList);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ServiceCompanyViewModel>();

        }

        public async Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int serviceCompanyID, string serviceCompanyCode)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            dictionary.Add("ServiceCompanyCode", serviceCompanyCode);
            var response = await _flurlClient.Request("/API/finishing/PendingRecover/GetPendingRecoveries")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<PendingRecoverDetailViewModel>();

        }

        public async Task<Result> RegistPendingRecover(int serviceCompanyID, string serviceCompanyCode, string recoverType, int userid)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            dictionary.Add("ServiceCompanyCode", serviceCompanyCode);
            dictionary.Add("RecoverType", recoverType);
            dictionary.Add("UserID", userid);
            var response = await _flurlClient.Request("/API/finishing/PendingRecover/RegistPendingRecover")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<Result>();

        }
    }
}
