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
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.Finishing.Repositories
{
    public class ExpeditionRepository : RepositoryBase, IExpeditionRepository
    {
        public ExpeditionRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<BusinessViewModel> GetCompanyBusiness(string ServiceCompanyList)
        {
            var response = await _flurlClient.Request("/API/finishing/Expedition/GetCompanyBusiness")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, ServiceCompanyList);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<BusinessViewModel>();

        }
        public async Task<ExpeditionFilesViewModel> GetPendingExpeditionFiles(int BusinessID, string ServiceCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyList", ServiceCompanyList);
            dictionary.Add("BusinessID", BusinessID);
            var response = await _flurlClient.Request("/API/finishing/Expedition/GetPendingRecoveries")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);

            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ExpeditionFilesViewModel>();

        }
    }
}
