using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Data.SqlClient;
using System.Data;
using evolUX.Interfaces;
using evolUX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace evolUX.Areas.evolDP.Controllers
{
    [Route("evoldp/envelopemedia/[action]")]
    [ApiController]
    public class EnvelopeMediaController : Controller
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerManager _logger;

        public EnvelopeMediaController(IWrapperRepository repositoryWrapper, ILoggerManager logger)
        {
            _repository = repositoryWrapper;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        [ActionName("get")]
        public async Task<ActionResult<List<dynamic>>> GetEnvelopeMedia()
        {
            try
            {
                var envelopeMediaList = await _repository.EnvelopeMedia.GetEnvelopeMedia();
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
        [ActionName("getOne")]
        public async Task<ActionResult<List<dynamic>>> GetEnvelopeMediaGroups()
        {
            try
            {
                var envelopeMediaGroupList = await _repository.EnvelopeMedia.GetEnvelopeMediaGroups();
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
