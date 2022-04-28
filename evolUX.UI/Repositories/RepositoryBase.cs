using Flurl.Http;
using Flurl.Http.Configuration;

namespace evolUX.UI.Repositories
{
    public class RepositoryBase
    {
        protected readonly IFlurlClient _flurlClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        //https://localhost:7107/ dev
        //http://localhost:5100/ prod

        public RepositoryBase(IFlurlClientFactory flurlClientFactory,
                              IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _flurlClient = flurlClientFactory.Get(_configuration.GetValue<string>("APIurl"));

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
