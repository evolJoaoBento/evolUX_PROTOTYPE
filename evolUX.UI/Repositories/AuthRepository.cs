using evolUX.UI.Areas.Core.Models;
using Flurl;
using Flurl.Http;
using System.Net;

namespace evolUX.UI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private const string BaseUrl = "https://localhost:7107/";
        public async Task<IFlurlResponse> GetTokenAndUser(string username)
        {
            //https://localhost:7107/ dev
            //http://localhost:5100/ prod
            try
            {
                var response = await BaseUrl
                     .AppendPathSegment($"/Core/Auth/login").SetQueryParam("username", username).AllowHttpStatus(HttpStatusCode.NotFound)
                     .GetAsync();

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
