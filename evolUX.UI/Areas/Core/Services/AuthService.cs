using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Areas.Core.Services.Interfaces;
using evolUX.UI.Repositories;
using Flurl.Http;
using System.Net;
using System.Xml.Linq;

namespace evolUX.UI.Areas.Core.Services
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
            var response = await _authRepository.GetTokenAndUser(username);
            return response;
        }

        public async Task<IFlurlResponse> LoginCredentials(string username, string password)
        {
            var response = await _authRepository.LoginCredentials(username, password);
            return response;
        }

        public async Task<IFlurlResponse> GetRefreshToken(string accessToken, string refreshToken)
        {
            var response = await _authRepository.GetRefreshToken(accessToken, refreshToken);
            return response;
        }

        public async Task<Dictionary<string, string>> GetSessionVariables(int ID)
        {
            var response = await _authRepository.GetSessionVariables(ID);
            return response;
        }
    }
}
