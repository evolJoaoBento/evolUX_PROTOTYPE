using evolUX.API.Areas.Core.ViewModels;
using evolUX.API.Areas.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using System.Security.Claims;
using System.Text;
using Shared.Models.Areas.Core;
using System.Data.SqlClient;
using System.IO;




namespace evolUX.API.Areas.Core.Controllers
{
    [Route("api/core/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILoggerService _logger;

        public AuthController(ILoggerService logger, IAuthenticationService authenticationService)
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

                //log information after login
                _logger.LogInfo($"User {username} logged in at {DateTime.Now}");
                return Ok(userAndToken);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside login action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [AllowAnonymous]
        [ActionName("logincredentials")]
        [HttpGet]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                
                if (model == null)
                {
                    return BadRequest("Invalid client request");
                }
                AuthResponse userAndToken = await _authenticationService.CredentialsAuth(new AuthRequest { Username = model.Username, Password = model.Password });
                if (userAndToken == null)
                {
                    return NotFound(new ErrorResult { Message = $"User {model.Username} Not Found", Code = 404 });
                }
                //log information after login
                _logger.LogInfo($"User {model.Username} logged in at {DateTime.Now}");
                return Ok(userAndToken);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
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
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside refresh action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

    }
}
