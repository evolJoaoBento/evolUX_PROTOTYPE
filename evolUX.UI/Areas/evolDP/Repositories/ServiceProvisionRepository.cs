using evolUX.UI.Repositories;
using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using Flurl.Http;
using Flurl.Http.Configuration;
using System.Net;
using evolUX.UI.Exceptions;
using Shared.ViewModels.Areas.evolDP;
using evolUX.API.Models;
using Shared.ViewModels.Areas.Finishing;
using Newtonsoft.Json;
using Shared.Models.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Repositories
{
    public class ServiceProvisionRepository : RepositoryBase, IServiceProvisionRepository
    {
        public ServiceProvisionRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<Company> SetServiceCompany(Company serviceCompany)
        {
            var response = await _flurlClient.Request("/API/evolDP/Generic/SetCompany")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, JsonConvert.SerializeObject(serviceCompany));
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<Company>();
        }

        public async Task<IEnumerable<Company>> GetServiceCompanies(string serviceCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyList", serviceCompanyList);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServiceCompanies")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<Company>>();
        }

        public async Task<IEnumerable<Company>> GetServiceCompany(int serviceCompanyID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServiceCompanies")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<Company>>();
        }
        public async Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int? serviceCompanyID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyID", serviceCompanyID);

            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServiceCompanyRestrictions")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ServiceCompanyRestriction>>();
        }
        public async Task<IEnumerable<ServiceCompanyServiceResume>> GetServiceCompanyConfigsResume(int? serviceCompanyID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyID", serviceCompanyID);

            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServiceCompanyConfigsResume")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ServiceCompanyServiceResume>>();
        }
        public async Task SetServiceCompanyRestrictions(int serviceCompanyID, int materialTypeID, int materialPosition, int fileSheetsCutoffLevel, bool restrictionMode)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            dictionary.Add("MaterialPosition", materialPosition);
            dictionary.Add("FileSheetsCutoffLevel", fileSheetsCutoffLevel);
            dictionary.Add("RestrictionMode", restrictionMode);

            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/SetServiceCompanyRestrictions")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return;
        }
        public async Task<IEnumerable<ServiceCompanyService>> GetServiceCompanyConfigs(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            dictionary.Add("serviceTypeID", serviceTypeID);
            dictionary.Add("serviceID", serviceID);
            dictionary.Add("CostDate", costDate);
            var response = await _flurlClient.Request("/API/evolDP/Expedition/GetServiceCompanyConfigs")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ServiceCompanyService>>();
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

    }
}
