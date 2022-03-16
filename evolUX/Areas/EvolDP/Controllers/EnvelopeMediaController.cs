using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Data.SqlClient;
using System.Data;
using evolUX.Interfaces;
using evolUX.Models;

namespace evolUX.Areas.EvolDP.Controllers
{
    [Route("evoldp/[controller]/[action]")]
    [ApiController]
    public class EnvelopeMediaController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerManager _logger;

        public EnvelopeMediaController(IRepositoryWrapper repositoryWrapper, ILoggerManager logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        [HttpGet]
        [ActionName("get")]
        public async Task<ActionResult<List<dynamic>>> GetEnvelopeMedia()
        {
            try
            {
                var envelopeMediaList = await _repositoryWrapper.EnvelopeMedia.GetEnvelopeMedia();
                return Ok(envelopeMediaList);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [ActionName("getOne")]
        public async Task<ActionResult<List<dynamic>>> GetEnvelopeMediaGroups()
        {
            try
            {
                var envelopeMediaGroupList = await _repositoryWrapper.EnvelopeMedia.GetEnvelopeMediaGroups();
                return Ok(envelopeMediaGroupList);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
