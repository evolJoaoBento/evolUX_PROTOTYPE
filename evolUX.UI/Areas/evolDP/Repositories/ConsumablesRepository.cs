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
    public class ConsumablesRepository : RepositoryBase, IConsumablesRepository
    {
        public ConsumablesRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            var response = await _flurlClient.Request("/API/evolDP/Consumables/GetFulfillMaterialCodes")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SendJsonAsync(HttpMethod.Get, dictionary);
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<IEnumerable<FulfillMaterialCode>>();
        }

        //public async Task<ConsumablesTypeViewModel> GetConsumablesTypes(int? ConsumablesType, string expCompanyList)
        //{
        //    Dictionary<string, object> dictionary = new Dictionary<string, object>();
        //    dictionary.Add("ConsumablesType", ConsumablesType);
        //    dictionary.Add("ExpCompanyList", expCompanyList);
        //    var response = await _flurlClient.Request("/API/evolDP/Consumables/GetConsumablesTypes")
        //        .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
        //        .SendJsonAsync(HttpMethod.Get, dictionary);
        //    if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
        //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
        //    return await response.GetJsonAsync<ConsumablesTypeViewModel>();
        //}
        //public async Task<ConsumablesZoneViewModel> GetConsumablesZones(int? ConsumablesZone, string expCompanyList)
        //{
        //    Dictionary<string, object> dictionary = new Dictionary<string, object>();
        //    dictionary.Add("ConsumablesZone", ConsumablesZone);
        //    dictionary.Add("ExpCompanyList", expCompanyList);
        //    var response = await _flurlClient.Request("/API/evolDP/Consumables/GetConsumablesZones")
        //        .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
        //        .SendJsonAsync(HttpMethod.Get, dictionary);
        //    if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
        //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
        //    return await response.GetJsonAsync<ConsumablesZoneViewModel>();
        //}
    }
}
