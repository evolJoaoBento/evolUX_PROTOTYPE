using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.EvolDP.Services.Interfaces;

namespace evolUX.API.Areas.EvolDP.Controllers
{
    [ApiController]
    [Route("evoldp/[controller]")]
    public class ExpeditionTypeController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IExpeditionTypeService _expeditionTypeService;

        public ExpeditionTypeController(ILoggerManager logger, IExpeditionTypeService expeditionTypeService)
        {
            _logger = logger;
            _expeditionTypeService = expeditionTypeService;
        }

        // GET: api/<ExpeditionTypeController>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public async Task<ActionResult<List<dynamic>>> Get()
        {
            try
            {
                //var expeditionTypeList = await _repository.ExpeditionType.GetExpeditionTypes();
                var expeditionTypeList = await _expeditionTypeService.GetExpeditionTypes();
                _logger.LogInfo("Expedition Type Get");
                return Ok(expeditionTypeList);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetEnvelopeMedia action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }
    }
}
