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
using System.Collections;
using NuGet.Protocol;

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
        public async Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int serviceCompanyID)
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
        public async Task<IEnumerable<ServiceCompanyServiceResume>> GetServiceCompanyConfigsResume(int serviceCompanyID)
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
        public async Task SetServiceCompanyRestriction(int serviceCompanyID, int materialTypeID, int materialPosition, int fileSheetsCutoffLevel, bool restrictionMode)
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
            dictionary.Add("ServiceTypeID", serviceTypeID);
            dictionary.Add("ServiceID", serviceID);
            dictionary.Add("CostDate", costDate);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServiceCompanyConfigs")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ServiceCompanyService>>();
        }
        
        public async Task SetServiceCompanyConfig(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID, double serviceCost, string formula)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            dictionary.Add("ServiceTypeID", serviceTypeID);
            dictionary.Add("ServiceID", serviceID);
            dictionary.Add("CostDate", costDate);
            dictionary.Add("ServiceCost", serviceCost);
            dictionary.Add("Formula", formula);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/SetServiceCompanyConfig")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return;
        }

        public async Task<IEnumerable<ServiceElement>> GetServices(int serviceTypeID)
        {
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServices")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, serviceTypeID);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ServiceElement>>();
        }
        public async Task<IEnumerable<ServiceTypeElement>> GetAvailableServiceTypes()
        {
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetAvailableServiceTypes")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .GetAsync();
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ServiceTypeElement>>();

        }
        public async Task<ServiceTypeViewModel> GetServiceTypes()
        {
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServiceTypes")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .GetAsync();
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ServiceTypeViewModel>();

        }
        public async Task SetServiceType(int serviceTypeID, string serviceTypeCode, string serviceTypeDesc)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceTypeID", serviceTypeID);
            dictionary.Add("ServiceTypeCode", serviceTypeCode);
            dictionary.Add("ServiceTypeDesc", serviceTypeDesc);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/SetServiceType")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return;
        }
        public async Task<IEnumerable<int>> GetServiceCompanyList(int serviceTypeID, int serviceID, int costDate)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceTypeID", serviceTypeID);
            dictionary.Add("ServiceID", serviceID);
            dictionary.Add("CostDate", costDate);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServiceCompanyList")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<int>>();
        }
        public async Task<IEnumerable<ServiceTaskElement>> GetServiceTasks(int? serviceTaskID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (serviceTaskID != null)
                dictionary.Add("ServiceTaskID", serviceTaskID);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServiceTasks")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ServiceTaskElement>>();
        }
        public async Task SetServiceTask(int serviceTaskID, string serviceTaskCode, string serviceTaskDesc, int refServiceTaskID, int complementServiceTaskID, int externalExpeditionMode, string stationExceededDesc)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceTaskID", serviceTaskID);
            dictionary.Add("ServiceTaskCode", serviceTaskCode);
            dictionary.Add("ServiceTaskDesc", serviceTaskDesc);
            dictionary.Add("RefServiceTaskID", refServiceTaskID);
            dictionary.Add("ComplementServiceTaskID", complementServiceTaskID);
            dictionary.Add("ExternalExpeditionMode", externalExpeditionMode);
            dictionary.Add("StationExceededDesc", stationExceededDesc);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/SetServiceTask")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return;
        }
        public async Task<IEnumerable<ExpCodeElement>> GetExpCodes(int serviceTaskID, int expCompanyID, string expCode)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (serviceTaskID != 0)
                dictionary.Add("ServiceTaskID", serviceTaskID);
            if (serviceTaskID != 0)
                dictionary.Add("ExpCompanyID", expCompanyID);
            if (!string.IsNullOrEmpty(expCode))
                dictionary.Add("ExpCode", expCode);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetExpCodes")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ExpCodeElement>>();
        }
        public async Task DeleteServiceType(int serviceTaskID, int serviceTypeID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceTaskID", serviceTaskID);
            dictionary.Add("ServiceTypeID", serviceTypeID);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/DeleteServiceType")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return;
        }
        public async Task AddServiceType(int serviceTaskID, int serviceTypeID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceTaskID", serviceTaskID);
            dictionary.Add("ServiceTypeID", serviceTypeID);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/AddServiceType")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return;
        }
        public async Task<IEnumerable<ExpCenterElement>> GetExpCenters(string expCode, string serviceCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("expCode", expCode);
            dictionary.Add("ServiceCompanyList", serviceCompanyList);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetExpCenters")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ExpCenterElement>>();
        }
        public async Task<ExpeditionZoneViewModel> GetExpeditionZones()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpeditionZone", 0);
            var response = await _flurlClient.Request("/API/evolDP/Expedition/GetExpeditionZones")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ExpeditionZoneViewModel>();
        }
    }
}
