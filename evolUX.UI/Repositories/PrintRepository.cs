using Dapper;
using evolUX.API.Areas.Finishing.Models;
using evolUX.API.Areas.Finishing.ViewModels;
using evolUX.API.Extensions;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Net;

namespace evolUX.UI.Repositories
{
    public class PrintRepository : RepositoryBase, IPrintRepository
    {
        public PrintRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<ResoursesViewModel> GetPrinters(string profileList, string filesSpecs, bool ignoreProfiles)
        {
            List<string> list = new List<string>();
            list.Add(profileList);
            list.Add(filesSpecs);
            string ListJSON = JsonConvert.SerializeObject(list);
            var response = await _flurlClient.Request("/API/finishing/Print/Printers")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SetQueryParam("ignoreProfiles",ignoreProfiles)
                .SendJsonAsync(HttpMethod.Get, ListJSON);
            //var response = await BaseUrl
            //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
            //     .GetAsync();
            if (response.StatusCode == ((int)HttpStatusCode.NotFound)) throw new HttpNotFoundException(response);
            if (response.StatusCode == ((int)HttpStatusCode.Unauthorized)) throw new HttpUnauthorizedException(response);
            return await response.GetJsonAsync<ResoursesViewModel>();
        }

    }
}
