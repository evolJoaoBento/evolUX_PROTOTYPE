using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using evolUX.Models;
using Dapper;
using evolUX.Interfaces;

namespace evolUX.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpeditionTypeController : ControllerBase
    {
        private readonly IExpeditionTypeRepository _expeditionTypeRepository;

        public ExpeditionTypeController(IExpeditionTypeRepository expeditionTypeRepository)
        {
            _expeditionTypeRepository = expeditionTypeRepository;
        }

        // GET: api/<ExpeditionTypeController>
        [HttpGet]
        public async Task<ActionResult<List<ExpeditionType>>> GetExpeditionType()
        {
            try
            {
                var expeditionTypeList = await _expeditionTypeRepository.GetExpeditionTypes();
                return Ok(expeditionTypeList);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
