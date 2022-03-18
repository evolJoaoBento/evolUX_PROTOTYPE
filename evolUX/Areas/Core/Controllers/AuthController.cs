using evolUX.Interfaces;
using evolUX.Models;
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
        [ActionName("GetToken")]
        [HttpGet]
        public IActionResult GetToken()
        {
            try
            {
                var username = User.Identity?.Name?.Split("\\")[1];
                var user = _repository.User.GetAllUsers().Result.SingleOrDefault(x => x.Username == username);
                if (user == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }
                //EXEMPLO
                user.Roles = "Officer";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Roles)
                };
                var token = _jwtService.GenerateJwtToken(claims);
                return Ok(new AuthenticateResponse(user, token));
            }
            catch(Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetToken action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPost]
        [ActionName("login")]
        public IActionResult Login([FromBody] AuthenticateRequest model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }
            var user = _repository.User.GetAllUsers().Result.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            //EXEMPLO
            user.Roles = "Officer";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Roles)
            };
            var token = _jwtService.GenerateJwtToken(claims);

            return Ok(new AuthenticateResponse(user, token));
        }
    }
}
