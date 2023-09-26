﻿using Shared.ViewModels.Areas.Finishing;
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
    public class DependentProductionRepository : RepositoryBase, IDependentProductionRepository
    {
        public DependentProductionRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<DependentProductionViewModel> GetDependentPrintsProduction(DataTable serviceCompanyList)
        {
            //Dictionary<string, object> dictionary = new Dictionary<string, object>();
            //dictionary.Add("ServiceCompanyList", serviceCompanyList);

            var response = await _flurlClient.Request("/API/reports/DependentProduction/GetDependentPrintsProduction")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, 3);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<DependentProductionViewModel>();
        }
    }
}