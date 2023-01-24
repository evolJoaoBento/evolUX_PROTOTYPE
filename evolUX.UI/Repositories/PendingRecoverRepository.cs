using Shared.ViewModels.Areas.Finishing;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using evolUX.UI.Repositories.Interfaces;

namespace evolUX.UI.Repositories
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
            if (response.StatusCode == ((int)HttpStatusCode.NotFound)) throw new HttpNotFoundException(response);
            if (response.StatusCode == ((int)HttpStatusCode.Unauthorized)) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ServiceCompanyViewModel>();
            
        }
        public async Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int serviceCompanyID)
        {
            var response = await _flurlClient.Request("/API/finishing/PendingRecover/GetPendingRecoveries")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SetQueryParams(new{
                    ServiceCompanyID = serviceCompanyID
                })
                .GetAsync();
            //var response = await BaseUrl
            //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
            //     .GetAsync();
            if (response.StatusCode == ((int)HttpStatusCode.NotFound)) throw new HttpNotFoundException(response);
            if (response.StatusCode == ((int)HttpStatusCode.Unauthorized)) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<PendingRecoverDetailViewModel>();
            
        }
    }
}
