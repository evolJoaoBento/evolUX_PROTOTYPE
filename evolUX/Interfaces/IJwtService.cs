using evolUX.Models;
using System.Security.Claims;

namespace evolUX.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(List<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
