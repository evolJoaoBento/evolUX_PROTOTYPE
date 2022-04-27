using evolUX.API.Areas.Core.Models;
using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Data.Interfaces;
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
        private readonly IAuthenticationService _authenticationService;
        private readonly ILoggerManager _logger;

        public AuthController(ILoggerManager logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [ActionName("login")]
        [HttpGet]
        public async Task<IActionResult> GetToken(string username)
        {
            try
            {
                if (username == null)
                {
                    return BadRequest("Invalid client request");
                }
                var userAndToken = await _authenticationService.WindowsAuth(new AuthRequest { Username = username, Password = "" });
                if (userAndToken == null)
                {
                    return NotFound(new ErrorResult { Message = $"User {username} Not Found", Code = 404 });
                }
                return Ok(userAndToken);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside login action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("logincredentials")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid client request");
                }
                var userAndToken = await _authenticationService.CredentialsAuth(new AuthRequest { Username = model.Username, Password = model.Password });
                if (userAndToken == null)
                {
                    return NotFound(new ErrorResult { Message = $"User {model.Username} Not Found", Code = 404 });
                }
                return Ok(userAndToken);
            }
            catch(Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Logincredentials action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiModel tokenApiModel)
        {
            try
            {
                if (tokenApiModel is null)
                {
                    return BadRequest("Invalid client request");
                }
                var result = await _authenticationService.RefreshToken(tokenApiModel.AccessToken, tokenApiModel.RefreshToken);
                if (result == null)
                {
                    return BadRequest("Refresh Token invalid");
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside refresh action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPost]
        [Authorize]
        [ActionName("revoke")]
        public IActionResult Revoke()
        {
            try
            {
                var username = User.Identity?.Name;
                _authenticationService.DeleteRefreshToken(username);
                return NoContent();
            }
            catch(Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside revoke action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

    }
}
