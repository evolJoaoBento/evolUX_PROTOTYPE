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

namespace evolUX.UI.Areas.evolDP.Repositories
{
    public class MaterialsRepository : RepositoryBase, IMaterialsRepository
    {
        public MaterialsRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            var response = await _flurlClient.Request("/API/evolDP/Materials/GetFulfillMaterialCodes")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<FulfillMaterialCode>>();
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
        public async Task<IEnumerable<MaterialElement>> GetMaterialGroups(string materialTypeCode)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("MaterialTypeCode", materialTypeCode);
            var response = await _flurlClient.Request("/API/evolDP/Materials/GetMaterialGroups")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int) HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int) HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<MaterialElement>>();
        }

        public async Task<IEnumerable<MaterialElement>> GetMaterials(int groupID, string materialTypeCode)
        {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("GroupID", groupID);
                dictionary.Add("MaterialTypeCode", materialTypeCode);
                var response = await _flurlClient.Request("/API/evolDP/Materials/GetMaterials")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                    .SendJsonAsync(HttpMethod.Get, dictionary);
                if (response.StatusCode == (int) HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
                if (response.StatusCode == (int) HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
                return await response.GetJsonAsync<IEnumerable<MaterialElement>>();
        }
    }
}
