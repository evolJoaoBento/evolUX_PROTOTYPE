using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using System.Data.SqlClient;

namespace evolUX.API.Areas.evolDP.Controllers
{
    [ApiController]
    [Route("api/evoldp/expeditiontype/[action]")]
    public class ExpeditionTypeController : ControllerBase
    {
        private readonly ILoggerService _logger;
        private readonly IExpeditionTypeService _expeditionTypeService;

        public ExpeditionTypeController(ILoggerService logger, IExpeditionTypeService expeditionTypeService)
        {
            _logger = logger;
            _expeditionTypeService = expeditionTypeService;
        }

        [HttpGet]
        [ActionName("GetExpeditionTypes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<dynamic>>> Get()
        {
            try
            {
                //var expeditionTypeList = await _repository.ExpeditionType.GetExpeditionTypes();
                var expeditionTypeList = await _expeditionTypeService.GetExpeditionTypes();
                _logger.LogInfo("Expedition Type Get");
                return Ok(expeditionTypeList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
