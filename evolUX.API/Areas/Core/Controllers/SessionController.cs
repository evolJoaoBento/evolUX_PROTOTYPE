using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
//using evolUX.API.Areas.Finishing.Models;
using evolUX.API.Areas.Finishing.ViewModels;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Areas.Core.Controllers
{
    [Route("core/auth/[action]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerManager _logger;
        private readonly ISessionService _sessionService;
        public SessionController(IWrapperRepository repository, ILoggerManager logger, ISessionService sessionService)
        {
            _repository = repository;
            _logger = logger;
            _sessionService = sessionService;
        }


        //MISSING SYSTEM VARIABLES & FLOW VARIABLES & ACTION PARAMETERS(XML READ)
        [HttpGet]
        [ActionName("SessionVariables")]
        public async Task<ActionResult<Dictionary<string,object>>> GetSessionVariables([FromBody] int User)
        {
            try
            {
                Dictionary<string,object> result = new Dictionary<string,object>();
                string profile = await _sessionService.GetProfile(User);
                result.Add("evolDP/Profile", profile);
                IEnumerable<string> servers = await _sessionService.GetServers(profile);
                DataTable serviceCompanies = await _sessionService.GetServiceCompanies(servers);
                result.Add("evolDP/ServiceCompanies", serviceCompanies);
                _logger.LogInfo("SessionVariables Get");
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside SessionVariables Get action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
