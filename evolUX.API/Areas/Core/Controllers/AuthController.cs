using evolUX.API.Areas.Core.Models;
using evolUX.API.Data.Interfaces;
using evolUX.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using System.Security.Claims;
using System.Text;

namespace evolUX.API.Areas.Core.Controllers
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

        [AllowAnonymous]
        [ActionName("login")]
        [HttpGet]
        public IActionResult GetToken(string username)
        {
            try
            {
                var user = _repository.User.GetUserByUsername(username).Result;
                if (user == null)
                {
                    return NotFound(new ErrorResult { Message = $"User {username} Not Found", Code = 404 });
                }
                //Roles
                var roles = _repository.User.GetRolesByUsername(username);
                user.Roles = roles.Result;

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

                _repository.User.UpdateUserRefreshTokenAndTime(user);

                return Ok(new AuthenticateResponse(user, accessToken.Token));
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
            var user = _repository.User.GetAllUsers().Result.SingleOrDefault(u => u.Username == username);
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
