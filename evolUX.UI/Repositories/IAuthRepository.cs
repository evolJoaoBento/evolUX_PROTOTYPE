using evolUX.UI.Areas.Core.Models;
using Flurl.Http;

namespace evolUX.UI.Repositories
{
    public interface IAuthRepository
    {
        Task<IFlurlResponse> GetTokenAndUser(string username);
    }
}
