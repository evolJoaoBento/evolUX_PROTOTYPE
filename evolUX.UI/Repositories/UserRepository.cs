using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Exceptions;
using evolUX.UI.Repositories.Interfaces;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Shared.ViewModels.Areas.Finishing;
using System.Net;

namespace evolUX.UI.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task ChangeCulture(string culture)
        {
            var response = await _flurlClient.Request("/API/Finishing/Print/Printers")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                .SetQueryParam("culture", culture)
                .PostAsync();
            //var response = await BaseUrl
            //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
            //     .GetAsync();
            if (response.StatusCode == ((int)HttpStatusCode.NotFound)) throw new HttpNotFoundException(response);
            if (response.StatusCode == ((int)HttpStatusCode.Unauthorized)) throw new HttpUnauthorizedException(response);
        }
        
    }
}
