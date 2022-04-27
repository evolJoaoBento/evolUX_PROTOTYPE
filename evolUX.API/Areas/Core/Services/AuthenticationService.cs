using evolUX.API.Areas.Core.Models;
using evolUX.API.Areas.Core.Services.Interfaces;
using System.Security.Authentication;
using System.Security.Claims;

namespace evolUX.API.Areas.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasherService _passwordHasherService;
        public AuthenticationService(IUserService userService, IJwtService jwtService, IPasswordHasherService passwordHasherService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _passwordHasherService = passwordHasherService;
        }
        
        public async Task<AuthResponse> WindowsAuth(AuthRequest request)
        {
            var user = await _userService.LoginUserWindows(new LoginRequest { Password = request.Password, Username = request.Username });
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                };
                claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Description)));
                var accessToken = new TokenResponse();
                accessToken.Token = _jwtService.GenerateJwtToken(claims);
                var refreshToken = _jwtService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                await _userService.UpdateUserRefreshTokenAndTime(user);
                return new AuthResponse(user, accessToken.Token);
            }
            return null;
        }

        public async Task<AuthResponse> CredentialsAuth(AuthRequest request)
        {
            var user = await _userService.LoginUserCredentials(new LoginRequest { Password = request.Password, Username = request.Username });
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                };
                claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Description)));
                var accessToken = new TokenResponse();
                accessToken.Token = _jwtService.GenerateJwtToken(claims);
                var refreshToken = _jwtService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                await _userService.UpdateUserRefreshTokenAndTime(user);
                return new AuthResponse(user, accessToken.Token);
            }
            throw new AuthenticationException();
        }

        public async Task<object> RefreshToken(string accessToken, string refreshToken)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity?.Name; //this is mapped to the Name claim by default
            //user o UserService
            var user = _userService.GetUserByUsername(username).Result;
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new {message= "Invalid client request" };
            }
            var newAccessToken = _jwtService.GenerateJwtToken(principal.Claims.ToList());
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _userService.UpdateUserRefreshToken(username, newRefreshToken);

            return new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            };
        }

        public async Task DeleteRefreshToken(string username)
        {
            await _userService.DeleteRefreshToken(username);
        }
    }
}
