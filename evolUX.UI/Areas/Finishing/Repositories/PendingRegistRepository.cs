using Shared.ViewModels.Areas.Finishing;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Repositories;

namespace evolUX.UI.Areas.Finishing.Repositories
{
    public class PendingRegistRepository : RepositoryBase, IPendingRegistRepository
    {
        public PendingRegistRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<PendingRegistViewModel> GetPendingRegist(string ServiceCompanyList)
        {
            var response = await _flurlClient.Request("/API/finishing/PendingRegist/GetPendingRegist")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, ServiceCompanyList);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<PendingRegistViewModel>();

        }
        public async Task<PendingRegistDetailViewModel> GetPendingRegistDetail(int runID, string ServiceCompanyList)
        {
            var response = await _flurlClient.Request("/API/finishing/PendingRegist/GetPendingRegistDetail")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SetQueryParam("RunID", runID)
                .SendJsonAsync(HttpMethod.Get, ServiceCompanyList);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<PendingRegistDetailViewModel>();

        }
    }
}
