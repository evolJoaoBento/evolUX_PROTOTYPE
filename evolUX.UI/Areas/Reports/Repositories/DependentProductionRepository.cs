using evolUX.UI.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using System.Data;
using System.Net;
using evolUX.UI.Repositories;
using evolUX.UI.Areas.Reports.Repositories.Interfaces;
using Shared.ViewModels.Areas.Reports;

namespace evolUX.UI.Areas.Reports.Repositories
{
    public class DependentProductionRepository : RepositoryBase, IDependentProductionRepository
    {
        public DependentProductionRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<DependentProductionViewModel> GetDependentPrintsProduction(DataTable serviceCompanyList)
        {
            DataTable RunIDList = new DataTable();
            RunIDList.Columns.Add("ID", typeof(int));

            // Suponho que serviceCompanyList seja um DataTable
            foreach (DataRow row in serviceCompanyList.Rows)
            {
                // Suponho que a coluna que você deseja acessar seja "ID"
                int runID;
                if (int.TryParse(row["ID"].ToString(), out runID))
                {
                    RunIDList.Rows.Add(runID);
                }
            }

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyList", RunIDList);

            var response = await _flurlClient.Request("/API/Reports/DependentProduction/Index")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<DependentProductionViewModel>();
        }
    }
}
