using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using System.Data.SqlClient;
using evolUX.API.Areas.evolDP.Services;
using Shared.Models.Areas.evolDP;

namespace evolUX.API.Areas.evolDP.Controllers
{
    [Route("api/evoldp/consumables/[action]")]
    [ApiController]
    public class ConsumablesController : Controller
    {
        private readonly ILoggerService _logger;
        private readonly IConsumablesService _consumables;

        public ConsumablesController(ILoggerService logger, IConsumablesService consumables)
        {
            _logger = logger;
            _consumables = consumables;
        }
        
        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetFulfillMaterialCodes")]
        public async Task<ActionResult<IEnumerable<FulfillMaterialCode>>> GetFulfillMaterialCodes([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("FullFillMaterialCode", out obj);
                string fullFillMaterialCode = Convert.ToString(obj).ToString();

                var result = await _consumables.GetFulfillMaterialCodes(fullFillMaterialCode);
                _logger.LogInfo("Expedition Company Configs Get");
                return Ok(result);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetExpeditionCompanyConfigs action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        [ActionName("GetEnvelopeMedia")]
        public async Task<ActionResult<List<dynamic>>> GetEnvelopeMedia()
        {
            try
            {
                //var envelopeMediaList = await _repository.EnvelopeMedia.GetEnvelopeMedia();
                var envelopeMediaList = await _consumables.GetEnvelopeMedia(null);
                _logger.LogInfo("Envelope Media Get");
                return Ok(envelopeMediaList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetEnvelopeMedia action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        [ActionName("GetEnvelopeMediaGroups")]
        public async Task<ActionResult<List<dynamic>>> GetEnvelopeMediaGroups()
        {
            try
            {
                var envelopeMediaGroupList = await _consumables.GetEnvelopeMediaGroups(null);
                _logger.LogInfo("Return envelope media group list from database");
                return Ok(envelopeMediaGroupList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetEnvelopeMediaGroups action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }
    }
}
