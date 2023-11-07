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
using System.Reflection.Emit;

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
            dictionary.Add("CompanyList", serviceCompanyList);
            var response = await _flurlClient.Request("/API/evolDP/Generic/GetCompanies")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<Company>>();
        }

        public async Task<IEnumerable<Company>> GetServiceCompany(int serviceCompanyID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("CompanyID", serviceCompanyID);
            var response = await _flurlClient.Request("/API/evolDP/Generic/GetCompanies")
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
            if (externalExpeditionMode >= 0)
                dictionary.Add("ExternalExpeditionMode", externalExpeditionMode);
            dictionary.Add("StationExceededDesc", stationExceededDesc);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/SetServiceTask")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return;
        }
        public async Task<IEnumerable<ExpCodeElement>> GetExpCodes(int serviceTaskID, string expCompanyList, string expCode)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (serviceTaskID != 0)
                dictionary.Add("ServiceTaskID", serviceTaskID);
            if (!string.IsNullOrEmpty(expCompanyList))
                dictionary.Add("ExpCompanyList", expCompanyList);
            if (!string.IsNullOrEmpty(expCode))
                dictionary.Add("ExpCode", expCode);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetExpCodes")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ExpCodeElement>>();
        }
        public async Task<IEnumerable<ExpCodeElement>> GetExpCodes(string expCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpCompanyList", expCompanyList);
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
            dictionary.Add("ExpCode", expCode);
            dictionary.Add("ServiceCompanyList", serviceCompanyList);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetExpCenters")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ExpCenterElement>>();
        }
        public async Task<IEnumerable<ServiceCompanyExpCodeElement>> GetServiceCompanyExpCodes(int serviceCompanyID, string expCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            dictionary.Add("ExpCompanyList", expCompanyList);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServiceCompanyExpCodes")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ServiceCompanyExpCodeElement>>();
        }

        public async Task<ExpeditionZoneViewModel> GetExpeditionZones(int expCompanyID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpCompanyID", expCompanyID);
            var response = await _flurlClient.Request("/API/evolDP/Expedition/GetExpeditionZones")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ExpeditionZoneViewModel>();
        }
        public async Task SetExpCenter(string expCode, string expCenterCode, string description1, string description2, string description3, int serviceCompanyID, string expeditionZone)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpCode", expCode);
            dictionary.Add("ExpCenterCode", expCenterCode);
            dictionary.Add("Description1", description1);
            dictionary.Add("Description2", description2);
            dictionary.Add("Description3", description3);
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            dictionary.Add("ExpeditionZone", expeditionZone);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/SetExpCenter")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return;
        }
        public async Task<IEnumerable<FullfillMaterialCode>> GetFulfillMaterialCodes()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            var response = await _flurlClient.Request("/API/evolDP/Materials/GetFulfillMaterialCodes")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<FullfillMaterialCode>>();
        }
        public async Task<IEnumerable<ServiceCompanyExpCodeConfig>> GetServiceCompanyExpCodeConfigs(string expCode, int serviceCompanyID, string expCenterCode)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpCode", expCode);
            dictionary.Add("ExpCenterCode", expCenterCode);
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/GetServiceCompanyExpCodeConfigs")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ServiceCompanyExpCodeConfig>>();
        }
        public async Task SetServiceCompanyExpCodeConfig(string expCode, int serviceCompanyID, string expCenterCode, int expLevel, string fullFillMaterialCode, int docMaxSheets, string barcode)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpCode", expCode);
            dictionary.Add("ExpCenterCode", expCenterCode);
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            dictionary.Add("ExpLevel", expLevel);
            dictionary.Add("FullFillMaterialCode", fullFillMaterialCode);
            dictionary.Add("DocMaxSheets", docMaxSheets);
            dictionary.Add("Barcode", barcode);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/SetServiceCompanyExpCodeConfig")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return;
        }
        public async Task DeleteServiceCompanyExpCodeConfig(string expCode, int serviceCompanyID, string expCenterCode, int expLevel)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpCode", expCode);
            dictionary.Add("ExpCenterCode", expCenterCode);
            dictionary.Add("ServiceCompanyID", serviceCompanyID);
            dictionary.Add("ExpLevel", expLevel);
            var response = await _flurlClient.Request("/API/evolDP/ServiceProvision/DeleteServiceCompanyExpCodeConfig")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return;
        }
    }
}
