﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.EvolDP.Services.Interfaces;

namespace evolUX.API.Areas.EvolDP.Controllers
{
    [ApiController]
    [Route("evoldp/expeditionzone/[action]")]
    public class ExpeditionZoneController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IExpeditionZoneService _expeditionZoneService;

        public ExpeditionZoneController(ILoggerManager logger, IExpeditionZoneService expeditionZoneService)
        {
            _logger = logger;
            _expeditionZoneService = expeditionZoneService;
        }

        [HttpGet]
        [ActionName("GetExpeditionZones")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<dynamic>>> Get()
        {
            try
            {
                //var expeditionTypeList = await _repository.ExpeditionType.GetExpeditionTypes();
                var expeditionZoneList = await _expeditionZoneService.GetExpeditionZones();
                _logger.LogInfo("Expedition Zone Get");
                return Ok(expeditionZoneList);
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
