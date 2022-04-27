using Flurl.Http;
using Flurl.Http.Configuration;

namespace evolUX.UI.Repositories
{
    public class RepositoryBase
    {
        protected readonly IFlurlClient _flurlClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //https://localhost:7107/ dev
        //http://localhost:5100/ prod

        public RepositoryBase(IFlurlClientFactory flurlClientFactory,
                              IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _flurlClient = flurlClientFactory.Get("https://localhost:7107/");

            _flurlClient.BeforeCall(flurlCall =>
            {

                var token = _httpContextAccessor.HttpContext.Request
                                .Cookies["X-Access-Token"];
                if (!string.IsNullOrWhiteSpace(token))
                {
                    flurlCall.HttpRequestMessage.SetHeader("Authorization", $"Bearer {token}");
                }
                else
                {
                    flurlCall.HttpRequestMessage.SetHeader("Authorization", string.Empty);
                }
            });
        }
    }
}
