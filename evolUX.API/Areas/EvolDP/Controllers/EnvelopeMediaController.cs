using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Data.Interfaces;
using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.EvolDP.Services.Interfaces;

namespace evolUX.API.Areas.EvolDP.Controllers
{
    [Route("api/evoldp/envelopemedia/[action]")]
    [ApiController]
    public class EnvelopeMediaController : Controller
    {
        private readonly ILoggerService _logger;
        private readonly IEnvelopeMediaService _envelopeMediaService;

        public EnvelopeMediaController(ILoggerService logger, IEnvelopeMediaService envelopeMediaService)
        {
            _logger = logger;
            _envelopeMediaService = envelopeMediaService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        [ActionName("GetEnvelopeMedia")]
        public async Task<ActionResult<List<dynamic>>> GetEnvelopeMedia()
        {
            try
            {
                //var envelopeMediaList = await _repository.EnvelopeMedia.GetEnvelopeMedia();
                var envelopeMediaList = await _envelopeMediaService.GetEnvelopeMedia();
                _logger.LogInfo("Envelope Media Get");
                return Ok(envelopeMediaList);
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
                //var envelopeMediaGroupList = await _repository.EnvelopeMedia.GetEnvelopeMediaGroups();
                var envelopeMediaGroupList = await _envelopeMediaService.GetEnvelopeMediaGroups();
                _logger.LogInfo("Return envelope media group list from database");
                return Ok(envelopeMediaGroupList);
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
