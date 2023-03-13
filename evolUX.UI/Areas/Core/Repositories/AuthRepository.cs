using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Repositories;
using evolUX.UI.Areas.Core.Repositories.Interfaces;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using System.Net;

namespace evolUX.UI.Areas.Core.Repositories
{
    public class AuthRepository : RepositoryBase, IAuthRepository
    {
        public AuthRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(flurlClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<IFlurlResponse> GetTokenAndUser(string username)
        {
            //https://localhost:7107/ dev
            //http://localhost:5100/ prod
            var response = await _flurlClient.Request("/API/Core/Auth/login").SetQueryParam("username", username)
                .AllowHttpStatus(HttpStatusCode.NotFound)
                .GetAsync();
            return response;
        }

        public async Task<IFlurlResponse> LoginCredentials(string username, string password)
        {
            var userLogin = new UserLogin
            {
                Username = username,
                Password = password
            };
            var response = await _flurlClient.Request("/API/Core/Auth/logincredentials")
                .AllowHttpStatus(HttpStatusCode.NotFound)
                .SendJsonAsync(HttpMethod.Get, userLogin);
            return response;
        }

        public async Task<IFlurlResponse> GetRefreshToken(string accessToken, string refreshToken)
        {
            var data = new
            {
                accessToken,
                refreshToken
            };
            var response = await _flurlClient.Request("/API/Core/Auth/refresh")
                .AllowHttpStatus(HttpStatusCode.BadRequest)
                .PostJsonAsync(data);

            return response;
        }

        public async Task<Dictionary<string, string>> GetSessionVariables(int ID)
        {
            var response = await _flurlClient.Request("/API/Core/Session/SessionVariables")
                .SetQueryParam("User", ID)
                .GetAsync();

            return await response.GetJsonAsync<Dictionary<string, string>>();
        }
    }
}
