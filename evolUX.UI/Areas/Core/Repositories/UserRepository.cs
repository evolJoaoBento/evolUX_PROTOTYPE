using evolUX.UI.Areas.Core.Repositories.Interfaces;
using evolUX.UI.Exceptions;
using evolUX.UI.Repositories;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using System.Net;
using System.Reflection;

namespace evolUX.UI.Areas.Core.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task ChangeCulture(int userID, string culture)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("UserID", userID);
            dictionary.Add("Culture", culture);

            var response = await _flurlClient.Request("/API/Core/User/ChangeCulture")
                .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
                 .SendJsonAsync(HttpMethod.Get, dictionary);
            //var response = await BaseUrl
            //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
            //     .GetAsync();
            if (response.StatusCode == (int)HttpStatusCode.NotFound) throw new HttpNotFoundException(response);
            if (response.StatusCode == (int)HttpStatusCode.Unauthorized) throw new HttpUnauthorizedException(response);
        }

    }
}
