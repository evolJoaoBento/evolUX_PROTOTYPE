using evolUX.Interfaces;
using evolUX.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace evolUX.Areas.Core.Controllers
{
    [Route("core/auth/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly IJwtService _jwtService;
        private readonly ILoggerManager _logger;

        public AuthController(IWrapperRepository wrapperRepository, IJwtService jwtService, ILoggerManager logger)
        {
            _repository = wrapperRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
        [ActionName("login")]
        [HttpGet]
        public IActionResult GetToken()
        {
            try
            {
                var username = User.Identity?.Name?.Split("\\")[1];
                var user = _repository.User.GetAllUsers().Result.SingleOrDefault(x => x.UserName == username);
                if (user == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }
                //EXEMPLO
                user.Roles = "Manager";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Roles)
                };
                var accessToken = _jwtService.GenerateJwtToken(claims);
                var refreshToken = _jwtService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                _repository.User.UpdateUserRefreshTokenAndTime(user);

                return Ok(new AuthenticateResponse(user, accessToken));
            }
            catch(Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetToken action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPost]
        [ActionName("refresh")]
        public IActionResult Refresh([FromBody] TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
            {
                return BadRequest("Invalid client request");
            }
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity?.Name; //this is mapped to the Name claim by default
            var user = _repository.User.GetAllUsers().Result.SingleOrDefault(u => u.UserName == username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }
            var newAccessToken = _jwtService.GenerateJwtToken(principal.Claims.ToList());
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            
            _repository.User.UpdateUserRefreshToken(username, newRefreshToken);
            return new ObjectResult(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ActionName("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity?.Name;
            _repository.User.DeleteRefreshToken(username);
            return NoContent();
        }

        //[HttpPost]
        //[ActionName("login")]
        //public IActionResult Login([FromBody] AuthenticateRequest model)
        //{
        //    if (model == null)
        //    {
        //        return BadRequest("Invalid client request");
        //    }
        //    var user = _repository.User.GetAllUsers().Result.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

        //    if (user == null)
        //    {
        //        return BadRequest(new { message = "Username or password is incorrect" });
        //    }

        //    //EXEMPLO
        //    user.Roles = "Officer";

        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.Username),
        //        new Claim(ClaimTypes.Role, user.Roles)
        //    };
        //    var token = _jwtService.GenerateJwtToken(claims);

        //    return Ok(new AuthenticateResponse(user, token));
        //}
    }
}
