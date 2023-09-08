using Shared.ViewModels.Areas.Finishing;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Repositories;
using Shared.Models.Areas.Finishing;
using evolUX.UI.Areas.Reports.Repositories.Interfaces;
using Shared.ViewModels.Areas.Reports;

namespace evolUX.UI.Areas.Reports.Repositories
{
    public class RetentionReportRepository : RepositoryBase, IRetentionReportRepository
    {
        public RetentionReportRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<RetentionRunReportViewModel> GetRetentionRunReport(int BusinessAreaID, int RefDate)
        {
            var response = await _flurlClient.Request("/API/reports/RetentionReport/RetentionRunReport")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SetQueryParam("DateRef", RefDate.ToString())
                .SendJsonAsync(HttpMethod.Get, BusinessAreaID);
                
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<RetentionRunReportViewModel>();
        }

        public async Task<RetentionReportViewModel> GetRetentionReport(List<int> runIDList, int businessAreaID)
        {
            DataTable RunIDList = new DataTable();
            RunIDList.Columns.Add("ID", typeof(int));
            foreach (int runID in runIDList)
                RunIDList.Rows.Add(runID);

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("RunIDList", RunIDList);
            dictionary.Add("BusinessAreaID", businessAreaID);

            var response = await _flurlClient.Request("/API/reports/RetentionReport/GetRetentionReport")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<RetentionReportViewModel>();
        }

        public async Task<RetentionInfoReportViewModel> GetRetentionInfoReport(int RunID, int FileID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("RunID", RunID);
            dictionary.Add("FileID", FileID);

            var response = await _flurlClient.Request("/API/reports/RetentionReport/GetRetentionInfoReport")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<RetentionInfoReportViewModel>();
        }
    }
}
