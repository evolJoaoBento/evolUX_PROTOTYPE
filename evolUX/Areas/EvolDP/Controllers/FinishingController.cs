using evolUX.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace evolUX.Areas.EvolDP.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FinishingController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerManager _logger;
        public FinishingController(IRepositoryWrapper repositoryWrapper, ILoggerManager logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        // GET: api/<ExpeditionTypeController>
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetRuns()
        {
            try
            {
                var runs = await _repositoryWrapper.Finishing.GetRunsOngoing();
                return Ok(runs);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<dynamic>> GetPendingRegists()
        {
            try
            {
                var runs = await _repositoryWrapper.Finishing.GetPendingRegist(); 
                return Ok(runs);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
