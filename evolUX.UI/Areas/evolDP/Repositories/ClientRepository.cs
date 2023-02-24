using evolUX.UI.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using System.Net;
using evolUX.UI.Repositories;
using Shared.ViewModels.Areas.evolDP;
using evolUX.UI.Areas.evolDP.Repositories.Interfaces;

namespace evolUX.UI.Areas.evolDP.Repositories
{
    public class ClientRepository : RepositoryBase, IClientRepository
    {
        public ClientRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<BusinessViewModel> GetCompanyBusiness(string CompanyBusinessList)
        {
            var response = await _flurlClient.Request("/API/finishing/Expedition/GetCompanyBusiness")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, CompanyBusinessList);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<BusinessViewModel>();

        }
        public async Task<ProjectListViewModel> GetProjects(string CompanyBusinessList)
        {
            var response = await _flurlClient.Request("/API/evolDP/Client/GetProjects")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, CompanyBusinessList);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ProjectListViewModel>();

        }
    }
}
