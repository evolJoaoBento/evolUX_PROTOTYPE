using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using evolUX.Models;
using Dapper;
using evolUX.Interfaces;

namespace evolUX.Areas.EvolDP.Controllers
{
    [ApiController]
    [Route("evoldp/[controller]")]
    public class ExpeditionTypeController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerManager _logger;

        public ExpeditionTypeController(IRepositoryWrapper repositoryWrapper, ILoggerManager logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        // GET: api/<ExpeditionTypeController>
        [HttpGet]
        public async Task<ActionResult<List<dynamic>>> Get()
        {
            try
            {
                var expeditionTypeList = await _repositoryWrapper.ExpeditionType.GetExpeditionTypes();
                _logger.LogInfo("Expedition Type Get");
                return Ok(expeditionTypeList);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
