using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using evolUX.Models;
using Dapper;
using evolUX.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace evolUX.Areas.EvolDP.Controllers
{
    [ApiController]
    [Route("evoldp/[controller]")]
    public class ExpeditionTypeController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerManager _logger;

        public ExpeditionTypeController(IWrapperRepository repositoryWrapper, ILoggerManager logger)
        {
            _repository = repositoryWrapper;
            _logger = logger;
        }

        // GET: api/<ExpeditionTypeController>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public async Task<ActionResult<List<dynamic>>> Get()
        {
            try
            {
                var expeditionTypeList = await _repository.ExpeditionType.GetExpeditionTypes();
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
