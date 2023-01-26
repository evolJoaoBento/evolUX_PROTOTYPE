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
    public class ProductionReportRepository : RepositoryBase, IProductionReportRepository
    {
        public ProductionReportRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<ProductionRunReportViewModel> GetProductionRunReport(string ServiceCompanyList)
        {
            var response = await _flurlClient.Request("/API/finishing/ProductionReport/ProductionRunReport")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, ServiceCompanyList);
            //var response = await BaseUrl
            //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
            //     .GetAsync();
            if (response.StatusCode == ((int)HttpStatusCode.NotFound)) throw new HttpNotFoundException(response);
            if (response.StatusCode == ((int)HttpStatusCode.Unauthorized)) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ProductionRunReportViewModel>();
            
        }
        public async Task<ProductionReportViewModel> GetProductionReport(string profileList, int runID, int serviceCompanyID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ProfileList", profileList);
            dictionary.Add("RunID", runID);
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            var response = await _flurlClient.Request("/API/finishing/ProductionReport/ProductionReport")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            //var response = await BaseUrl
            //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
            //     .GetAsync();
            if (response.StatusCode == ((int)HttpStatusCode.NotFound)) throw new HttpNotFoundException(response);
            if (response.StatusCode == ((int)HttpStatusCode.Unauthorized)) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ProductionReportViewModel>();

        }
        //public async Task<ProductionReportViewModel> GetProductionReport(string profilesList, int runID, int serviceCompanyID)
        //{
        //    var response = await _flurlClient.Request("/API/finishing/ProductionReport/ProductionReport")
        //        .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
        //        .SetQueryParams(new{
        //            RunID = runID,
        //            ServiceCompanyID = serviceCompanyID
        //        })
        //        .GetAsync();
        //    //var response = await BaseUrl
        //    //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
        //    //     .GetAsync();
        //    if (response.StatusCode == ((int)HttpStatusCode.NotFound)) throw new HttpNotFoundException(response);
        //    if (response.StatusCode == ((int)HttpStatusCode.Unauthorized)) throw new HttpUnauthorizedException(response);
        //    return await response.GetJsonAsync<ProductionReportViewModel>();

        //}
    }
}
