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
    public class ProductionReportRepository : RepositoryBase, IProductionReportRepository
    {
        public ProductionReportRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<ProductionRunReportViewModel> GetProductionRunReport(int ServiceCompanyID)
        {
            var response = await _flurlClient.Request("/API/finishing/ProductionReport/ProductionRunReport")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, ServiceCompanyID);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ProductionRunReportViewModel>();
        }
        public async Task<ProductionReportViewModel> GetProductionReport(string profileList, int runID, int serviceCompanyID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ProfileList", profileList);
            dictionary.Add("RunID", runID);
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            var response = await _flurlClient.Request("/API/finishing/ProductionReport/GetProductionReport")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ProductionReportViewModel>();
        }

        public async Task<ProductionReportPrinterViewModel> GetProductionPrinterReport(string profileList, int runID, int serviceCompanyID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ProfileList", profileList);
            dictionary.Add("RunID", runID);
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            var response = await _flurlClient.Request("/API/finishing/ProductionReport/GetProductionPrinterReport")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ProductionReportPrinterViewModel>();
        }
    }
}
