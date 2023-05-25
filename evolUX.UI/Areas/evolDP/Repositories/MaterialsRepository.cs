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
        public async Task<IEnumerable<MaterialType>> GetMaterialTypes()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            var response = await _flurlClient.Request("/API/evolDP/Materials/GetMaterialTypes")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<MaterialType>>();
        }
        //public async Task<MaterialsTypeViewModel> GetMaterialsTypes(int? MaterialsType, string expCompanyList)
        //{
        //    Dictionary<string, object> dictionary = new Dictionary<string, object>();
        //    dictionary.Add("MaterialsType", MaterialsType);
        //    dictionary.Add("ExpCompanyList", expCompanyList);
        //    var response = await _flurlClient.Request("/API/evolDP/Materials/GetMaterialsTypes")
        //        .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
        //        .SendJsonAsync(HttpMethod.Get, dictionary);
        //    if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
        //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
        //    return await response.GetJsonAsync<MaterialsTypeViewModel>();
        //}
        //public async Task<MaterialsZoneViewModel> GetMaterialsZones(int? MaterialsZone, string expCompanyList)
        //{
        //    Dictionary<string, object> dictionary = new Dictionary<string, object>();
        //    dictionary.Add("MaterialsZone", MaterialsZone);
        //    dictionary.Add("ExpCompanyList", expCompanyList);
        //    var response = await _flurlClient.Request("/API/evolDP/Materials/GetMaterialsZones")
        //        .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
        //        .SendJsonAsync(HttpMethod.Get, dictionary);
        //    if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
        //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
        //    return await response.GetJsonAsync<MaterialsZoneViewModel>();
        //}
    }
}
