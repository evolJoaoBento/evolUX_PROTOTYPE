using evolUX.UI.Repositories;
using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using Flurl.Http;
using Flurl.Http.Configuration;
using System.Net;
using evolUX.UI.Exceptions;
using Shared.ViewModels.Areas.evolDP;
using evolUX.API.Models;
using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.evolDP;
using evolUX_dev.Areas.evolDP.Models;
using Newtonsoft.Json;
using Shared.ViewModels.General;
using System.Collections;

namespace evolUX.UI.Areas.evolDP.Repositories
{
    public class ExpeditionRepository : RepositoryBase, IExpeditionRepository
    {
        public ExpeditionRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<Company> SetExpCompany(Company expCompany)
        {
            var response = await _flurlClient.Request("/API/evolDP/Generic/SetCompany")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, JsonConvert.SerializeObject(expCompany));
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<Company>();
        }
        
        public async Task<ExpeditionTypeViewModel> GetExpeditionCompanies(string expCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpCompanyList", expCompanyList);
            var response = await _flurlClient.Request("/API/evolDP/Expedition/GetExpeditionCompanies")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ExpeditionTypeViewModel>();
        }
        
        public async Task<ExpeditionTypeViewModel> GetExpeditionCompanies(int expCompanyID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpCompanyID", expCompanyID);
            var response = await _flurlClient.Request("/API/evolDP/Expedition/GetExpeditionCompanies")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ExpeditionTypeViewModel>();
        }
        public async Task<ExpeditionTypeViewModel> GetExpeditionTypes(int? expeditionType, string expCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpeditionType", expeditionType);
            dictionary.Add("ExpCompanyList", expCompanyList);
            var response = await _flurlClient.Request("/API/evolDP/Expedition/GetExpeditionTypes")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ExpeditionTypeViewModel>();
        }
        public async Task<IEnumerable<ExpCompanyType>> GetExpCompanyTypes(int? expeditionType, int? expCompanyID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpeditionType", expeditionType);
            dictionary.Add("ExpCompanyID", expCompanyID);
            var response = await _flurlClient.Request("/API/evolDP/Expedition/GetExpCompanyTypes")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ExpCompanyType>>();
        }
        public async Task<ResultsViewModel> SetExpCompanyType(int expeditionType, int expCompanyID, bool registMode, bool separationMode, bool barcodeRegistMode)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpeditionType", expeditionType);
            dictionary.Add("ExpCompanyID", expCompanyID);
            dictionary.Add("RegistMode", registMode);
            dictionary.Add("SeparationMode", separationMode);
            dictionary.Add("BarcodeRegistMode", barcodeRegistMode);
            var response = await _flurlClient.Request("/API/evolDP/Expedition/SetExpCompanyType")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ResultsViewModel>();
        }
        public async Task<ExpeditionZoneViewModel> GetExpeditionZones(int? expeditionZone, string expCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpeditionZone", expeditionZone);
            dictionary.Add("ExpCompanyList", expCompanyList);
            var response = await _flurlClient.Request("/API/evolDP/Expedition/GetExpeditionZones")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ExpeditionZoneViewModel>();
        }
        public async Task<IEnumerable<ExpeditionRegistElement>> GetExpeditionRegistIDs(int expCompanyID)
        {
           var response = await _flurlClient.Request("/API/evolDP/Expedition/GetExpeditionRegistIDs")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, expCompanyID);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ExpeditionRegistElement>>();
        }
        public async Task SetExpeditionRegistID(ExpeditionRegistElement expRegist)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpRegist", JsonConvert.SerializeObject(expRegist));
            var response = await _flurlClient.Request("/API/evolDP/Expedition/SetExpeditionRegistID")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return;
        }
        public async Task<IEnumerable<ExpContractElement>> GetExpContracts(int expCompanyID)
        {
            var response = await _flurlClient.Request("/API/evolDP/Expedition/GetExpContracts")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, expCompanyID);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ExpContractElement>>();
        }
        public async Task SetExpContract(ExpContractElement expContract)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpContract", JsonConvert.SerializeObject(expContract));
            var response = await _flurlClient.Request("/API/evolDP/Expedition/SetExpContract")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return;
        }
        public async Task<IEnumerable<ExpCompanyConfig>> GetExpCompanyConfigs(int expCompanyID, int startDate, int expeditionType, int expeditionZone)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpCompanyID", expCompanyID);
            dictionary.Add("ExpeditionType", expeditionType);
            dictionary.Add("ExpeditionZone", expeditionZone);
            dictionary.Add("StartDate", startDate);
            var response = await _flurlClient.Request("/API/evolDP/Expedition/GetExpCompanyConfigs")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ExpCompanyConfig>>();
        }
        public async Task<IEnumerable<ExpCompanyConfig>> SetExpCompanyConfig(ExpCompanyConfig expCompanyConfig)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpCompanyConfig", JsonConvert.SerializeObject(expCompanyConfig));
            var response = await _flurlClient.Request("/API/evolDP/Expedition/SetExpCompanyConfig")
                 .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ExpCompanyConfig>>();
        }

        public async Task<IEnumerable<ExpCompanyConfigResume>> GetExpCompanyConfigsResume(int expCompanyID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ExpCompanyID", expCompanyID);
            var response = await _flurlClient.Request("/API/evolDP/Expedition/GetExpCompanyConfigsResume")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<ExpCompanyConfigResume>>();
        }
    }
}
