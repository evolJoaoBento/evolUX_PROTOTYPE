﻿using evolUX.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace evolUX.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FinishingController : ControllerBase
    {
        private readonly IFinishingRepository _finishingRepository;
        public FinishingController(IFinishingRepository finishingRepository){
            _finishingRepository = finishingRepository;
        }

        // GET: api/<ExpeditionTypeController>
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetRuns()
        {
            try
            {
                var runs = await _finishingRepository.GetRunsOngoing();
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
                var runs = await _finishingRepository.GetPendingRegist(); ;
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
