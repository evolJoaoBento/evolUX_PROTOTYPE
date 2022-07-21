using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Data.Interfaces;
using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.EvolDP.Services.Interfaces;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Dynamic;

namespace evolUX.API.Areas.EvolDP.Controllers
{
    [Route("evoldp/envelopemedia/[action]")]
    [ApiController]
    public class ExpeditionCompaniesController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IExpeditionCompaniesService _expeditionCompaniesService;

        public ExpeditionCompaniesController (ILoggerManager logger, IExpeditionCompaniesService expeditionCompaniesService)
        {
            _logger = logger;
            _expeditionCompaniesService = expeditionCompaniesService;
        }
        //TODO: UNTESTED
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetExpeditionCompanies")]
        public async Task<ActionResult<List<dynamic>>> GetExpeditionCompanies()
        {
            try
            {
                var expeditionCompaniesList = await _expeditionCompaniesService.GetExpeditionCompanies();
                _logger.LogInfo("Expedition Companies Get");
                return Ok(expeditionCompaniesList);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetExpeditionCompanies action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        //TODO: UNTESTED
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetExpeditionCompanyConfigs")]
        public async Task<ActionResult<List<dynamic>>> GetExpeditionCompanyConfigs([FromBody] dynamic data)
        {
            try
            {
                var converter = new ExpandoObjectConverter();
                var exObjExpandoObject = JsonConvert.DeserializeObject<ExpandoObject>(data.ToString(), converter) as dynamic;
                var expeditionCompanyConfigsList = await _expeditionCompaniesService.GetExpeditionCompanyConfigs(exObjExpandoObject);
                _logger.LogInfo("Expedition Company Configs Get");
                return Ok(expeditionCompanyConfigsList);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetExpeditionCompanyConfigs action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        //TODO: UNTESTED
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetExpeditionCompanyConfigCharacteristics")]
        public async Task<ActionResult<List<dynamic>>> GetExpeditionCompanyConfigCharacteristics([FromBody] dynamic data)
        {
            try
            {
                var converter = new ExpandoObjectConverter();
                var exObjExpandoObject = JsonConvert.DeserializeObject<ExpandoObject>(data.ToString(), converter) as dynamic;
                var expeditionCompanyConfigsList = await _expeditionCompaniesService.GetExpeditionCompanyConfigCharacteristics(exObjExpandoObject);
                _logger.LogInfo("Expedition Company Config Characteristics Get");
                return Ok(expeditionCompanyConfigsList);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetExpeditionCompanyConfigCharacteristics action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
