using evolUX.UI.Areas.Core.Models;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using System.Net;

namespace evolUX.UI.Repositories
{
    public class AuthRepository : RepositoryBase, IAuthRepository
    {
        public AuthRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor) : base(flurlClientFactory, httpContextAccessor)
        {
        }

        public async Task<IFlurlResponse> GetTokenAndUser(string username)
        {
            //https://localhost:7107/ dev
            //http://localhost:5100/ prod
            try
            {
                var response = await _flurlClient.Request("/Core/Auth/login").SetQueryParam("username", username)
                    .AllowHttpStatus(HttpStatusCode.NotFound)
                    .GetAsync();
                //var response = await BaseUrl
                //     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
                //     .GetAsync();

                return response;
            }

            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                dynamic d = ex.GetResponseJsonAsync();
                return d;
            }
        }

        public async Task<IFlurlResponse> LoginCredentials(string username, string password)
        {
            try
            {
                var userLogin = new UserLogin
                {
                    Username = username,
                    Password = password
                };
                var response = await _flurlClient.Request("/Core/Auth/logincredentials")
                    .PostJsonAsync(userLogin);
                return response;
            }
            catch (FlurlHttpException ex)
            {
                dynamic d = ex.GetResponseJsonAsync();
                return d;
            }
        }

        public async Task<IFlurlResponse> GetRefreshToken(string accessToken, string refreshToken)
        {
            try
            {
                var data = new
                {
                    accessToken = accessToken,
                    refreshToken = refreshToken
                };
                var response = await _flurlClient.Request("/Core/Auth/refresh").PostJsonAsync(data);

                return response;
            }

            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                dynamic d = ex.GetResponseJsonAsync();
                return d;
            }
        }
    }
}
