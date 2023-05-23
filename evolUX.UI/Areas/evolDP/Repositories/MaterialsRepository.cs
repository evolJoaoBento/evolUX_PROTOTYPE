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
using Newtonsoft.Json;

namespace evolUX.UI.Areas.evolDP.Repositories
{
    public class MaterialsRepository : RepositoryBase, IMaterialsRepository
    {
        public MaterialsRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
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
        public async Task<IEnumerable<MaterialType>> GetMaterialTypes(bool groupCodes, string materialTypeCode)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("GroupCodes", groupCodes);
            dictionary.Add("MaterialTypeCode", materialTypeCode);

            var response = await _flurlClient.Request("/API/evolDP/Materials/GetMaterialTypes")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<MaterialType>>();
        }
        public async Task<IEnumerable<MaterialElement>> GetMaterialGroups(string materialTypeCode, string serviceCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("MaterialTypeCode", materialTypeCode);
            dictionary.Add("ServiceCompanyList", serviceCompanyList);
            var response = await _flurlClient.Request("/API/evolDP/Materials/GetMaterialGroups")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int) HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int) HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<MaterialElement>>();
        }
        public async Task<MaterialElement> SetMaterialGroup(MaterialElement material, string serviceCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("GroupJSON", JsonConvert.SerializeObject(material));
            dictionary.Add("ServiceCompanyList", serviceCompanyList);
            var response = await _flurlClient.Request("/API/evolDP/Materials/SetMaterialGroup")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Put, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<MaterialElement>(); ;
        }
        public async Task<IEnumerable<MaterialElement>> GetMaterials(int groupID, string materialTypeCode, string serviceCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("GroupID", groupID);
            dictionary.Add("MaterialTypeCode", materialTypeCode);
            dictionary.Add("ServiceCompanyList", serviceCompanyList);
            var response = await _flurlClient.Request("/API/evolDP/Materials/GetMaterials")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int) HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int) HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<IEnumerable<MaterialElement>>();
        }
        public async Task<MaterialElement> SetMaterial(MaterialElement material, string materialTypeCode, string serviceCompanyList)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(materialTypeCode))
                dictionary.Add("MaterialTypeCode", materialTypeCode);
            dictionary.Add("MaterialJSON", JsonConvert.SerializeObject(material));
            dictionary.Add("ServiceCompanyList", serviceCompanyList);
            var response = await _flurlClient.Request("/API/evolDP/Materials/SetMaterial")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Put, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<MaterialElement>();
        }
    }
}
