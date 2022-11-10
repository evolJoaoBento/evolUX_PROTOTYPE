using evolUX.API.Areas.Core.ViewModels;

namespace evolUX.API.Areas.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResponse> WindowsAuth(AuthRequest request);
        Task<AuthResponse> CredentialsAuth(AuthRequest request);
        Task<object> RefreshToken(string accessToken, string refreshToken);
        Task DeleteRefreshToken(string username);

    }
}
