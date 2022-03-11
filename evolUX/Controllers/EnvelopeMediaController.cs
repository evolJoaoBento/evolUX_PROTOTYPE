using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Data.SqlClient;
using System.Data;
using evolUX.Interfaces;
using evolUX.Models;

namespace evolUX.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EnvelopeMediaController : Controller
    {
        private IEnvelopeMediaRepository _envelopeMediaRepository;

        public EnvelopeMediaController(IEnvelopeMediaRepository envelopeMediaRepository)
        {
            _envelopeMediaRepository = envelopeMediaRepository;
        }

        [HttpGet]
        [ActionName("get")]
        public async Task<ActionResult<List<dynamic>>> GetEnvelopeMedia()
        {
            try
            {
                var envelopeMediaList = await _envelopeMediaRepository.GetEnvelopeMedia();
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
                var envelopeMediaGroupList = await _envelopeMediaRepository.GetEnvelopeMediaGroups();
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
