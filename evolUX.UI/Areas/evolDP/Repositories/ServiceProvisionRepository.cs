using evolUX.UI.Repositories;
using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using Flurl.Http;
using Flurl.Http.Configuration;
using System.Net;
using evolUX.UI.Exceptions;
using Shared.ViewModels.Areas.evolDP;
using evolUX.API.Models;
using Shared.ViewModels.Areas.Finishing;

namespace evolUX.UI.Areas.evolDP.Repositories
{
    public class ServiceProvisionRepository : RepositoryBase, IServiceProvisionRepository
    {
        public ServiceProvisionRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        //public async Task<ServiceProvisionTypeViewModel> GetServiceProvisionTypes(int? ServiceProvisionType, string expCompanyList)
        //{
        //    Dictionary<string, object> dictionary = new Dictionary<string, object>();
        //    dictionary.Add("ServiceProvisionType", ServiceProvisionType);
        //    dictionary.Add("ExpCompanyList", expCompanyList);
        //    var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServiceProvisionTypes")
        //        .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
        //        .SendJsonAsync(HttpMethod.Get, dictionary);
        //    if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
        //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
        //    return await response.GetJsonAsync<ServiceProvisionTypeViewModel>();
        //}
        //public async Task<ServiceProvisionZoneViewModel> GetServiceProvisionZones(int? ServiceProvisionZone, string expCompanyList)
        //{
        //    Dictionary<string, object> dictionary = new Dictionary<string, object>();
        //    dictionary.Add("ServiceProvisionZone", ServiceProvisionZone);
        //    dictionary.Add("ExpCompanyList", expCompanyList);
        //    var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServiceProvisionZones")
        //        .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
        //        .SendJsonAsync(HttpMethod.Get, dictionary);
        //    if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
        //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
        //    return await response.GetJsonAsync<ServiceProvisionZoneViewModel>();
        //}
    }
}
