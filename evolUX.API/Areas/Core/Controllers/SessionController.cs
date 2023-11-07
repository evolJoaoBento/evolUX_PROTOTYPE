using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using Shared.Models.Areas.Finishing;
using Shared.ViewModels.Areas.Finishing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using System.Data;
using Newtonsoft.Json;
using Shared.Models.Areas.Core;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using System.Data.SqlClient;

namespace evolUX.API.Areas.Core.Controllers
{
    [Route("api/core/[controller]/[action]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        //private readonly ILoggerService _logger;
        private readonly ISessionService _sessionService;
        public SessionController(IWrapperRepository repository, ILoggerService logger, ISessionService sessionService)
        {
            _repository = repository;
            //_logger = logger;
            _sessionService = sessionService;
        }


        //MISSING SYSTEM VARIABLES & FLOW VARIABLES & ACTION PARAMETERS(XML READ)
        [HttpGet]
        [ActionName("SessionVariables")]
        public async Task<ActionResult<Dictionary<string,string>>> GetSessionVariables([FromQuery] int User)
        {
            try
            {
                Dictionary<string,string> result = await _sessionService.GetSessionVariables(User);
                //TODO: PermissionLevel
                //_logger.LogInfo("SessionVariables Get");
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(401, "No Profiles Found");
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                //_logger.LogError($"Something went wrong inside SessionVariables Get action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
