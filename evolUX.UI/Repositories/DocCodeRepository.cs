using Flurl.Http;
using Flurl.Http.Configuration;
using System.Net;

namespace evolUX.UI.Repositories
{
    public class DocCodeRepository : RepositoryBase, IDocCodeRepository
    {
        public DocCodeRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor) : base(flurlClientFactory, httpContextAccessor)
        {
        }

        public async Task<IFlurlResponse> GetDocCode()
        {
            try
            {
                var response = await _flurlClient.Request("/evoldp/doccode/getDocCode")
                    .AllowHttpStatus(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized)
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
    }
}
