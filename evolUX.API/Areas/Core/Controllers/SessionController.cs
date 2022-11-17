using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
//using Shared.Models.Areas.Finishing;
using Shared.ViewModels.Areas.Finishing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using System.Data;
using Newtonsoft.Json;

namespace evolUX.API.Areas.Core.Controllers
{
    [Route("api/core/[controller]/[action]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly ISessionService _sessionService;
        public SessionController(IWrapperRepository repository, ILoggerService logger, ISessionService sessionService)
        {
            _repository = repository;
            _logger = logger;
            _sessionService = sessionService;
        }


        //MISSING SYSTEM VARIABLES & FLOW VARIABLES & ACTION PARAMETERS(XML READ)
        [HttpGet]
        [ActionName("SessionVariables")]
        public async Task<ActionResult<Dictionary<string,string>>> GetSessionVariables([FromQuery] int User)
        {
            try
            {
                Dictionary<string,string> result = new Dictionary<string,string>();
                IEnumerable<int> profiles = await _sessionService.GetProfile(User);
                result.Add("evolDP/Profiles", JsonConvert.SerializeObject(profiles));
                IEnumerable<string> servers = await _sessionService.GetServers(profiles);
                DataTable serviceCompanies = await _sessionService.GetServiceCompanies(servers);
                //TODO: PermissionLevel
                _logger.LogInfo("SessionVariables Get");
                result.Add("evolDP/ServiceCompanies", JsonConvert.SerializeObject(serviceCompanies));
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
