using Shared.ViewModels.Areas.Finishing;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System.Net;
using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using Shared.Models.General;
using evolUX.UI.Repositories;
using Shared.ViewModels.Areas.evolDP;
using Shared.Models.Areas.Finishing;

namespace evolUX.UI.Areas.Finishing.Repositories
{
    public class ExpeditionReportRepository : RepositoryBase, IExpeditionReportRepository
    {
        public ExpeditionReportRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<BusinessViewModel> GetCompanyBusiness(string ServiceCompanyList)
        {
            var response = await _flurlClient.Request("/API/finishing/ExpeditionReport/GetCompanyBusiness")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, ServiceCompanyList);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<BusinessViewModel>();

        }
        public async Task<ExpeditionListViewModel> GetPendingExpeditionFiles(int BusinessID, string ServiceCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyList", ServiceCompanyList);
            dictionary.Add("BusinessID", BusinessID);
            var response = await _flurlClient.Request("/API/finishing/ExpeditionReport/GetPendingExpeditionFiles")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);

            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ExpeditionListViewModel>();

        }
        public async Task<Result> RegistExpeditionReport(List<RegistExpReportElement> expFiles, string username, int userID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("Username", username);
            dictionary.Add("UserID", userID);
            string ListJSON = JsonConvert.SerializeObject(expFiles);
            dictionary.Add("ExpFiles", ListJSON);

            var response = await _flurlClient.Request("/API/Finishing/ExpeditionReport/RegistExpeditionReport")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<Result>();
        }
        public async Task<ExpeditionListViewModel> GetExpeditionReportList(int BusinessID, string ServiceCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyList", ServiceCompanyList);
            dictionary.Add("BusinessID", BusinessID);
            var response = await _flurlClient.Request("/API/finishing/ExpeditionReport/GetExpeditionReportList")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);

            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ExpeditionListViewModel>();

        }
    }
}
