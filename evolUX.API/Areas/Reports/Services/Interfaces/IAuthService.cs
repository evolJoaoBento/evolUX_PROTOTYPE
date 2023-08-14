using evolUX.UI.Areas.Core.Models;
using Flurl.Http;

namespace evolUX.UI.Areas.Core.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IFlurlResponse> GetTokenAndUser(string username);
        Task<IFlurlResponse> LoginCredentials(string username, string password);
        Task<IFlurlResponse> GetRefreshToken(string accessToken, string refreshToken);
        Task<Dictionary<string, string>> GetSessionVariables(int ID);
    }
}
