
using System.Security.Claims;

namespace evolUX.API.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(List<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
