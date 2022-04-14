using evolUX.UI.Areas.Core.Models;
using Flurl.Http;

namespace evolUX.UI.Services
{
    public interface IAuthService
    {
         Task<IFlurlResponse> GetTokenAndUser(string username);
    }
}
