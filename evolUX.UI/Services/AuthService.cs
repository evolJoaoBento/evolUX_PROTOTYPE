using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Repositories;
using Flurl.Http;

namespace evolUX.UI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<IFlurlResponse> GetTokenAndUser(string username)
        {
            return await _authRepository.GetTokenAndUser(username);
        }
    }
}
