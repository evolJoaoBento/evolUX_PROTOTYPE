using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using System.Data.SqlClient;
using evolUX.API.Areas.evolDP.Services;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Dynamic;
using Shared.ViewModels.Areas.evolDP;
using System.Collections.Generic;
using System.Data;

namespace evolUX.API.Areas.evolDP.Controllers
{
    [ApiController]
    [Route("api/evoldp/[controller]/[action]")]
    public class ExpeditionController : ControllerBase
    {
        private readonly ILoggerService _logger;
        private readonly IExpeditionService _expeditionService;

        public ExpeditionController(ILoggerService logger, IExpeditionService expeditionService)
        {
            _logger = logger;
            _expeditionService = expeditionService;
        }

        [HttpGet]
        [ActionName("GetExpeditionTypes")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ExpeditionTypeViewModel>> GetExpeditionTypes([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ExpeditionType", out obj);
                int value = 0;
                int? expeditionType = null;
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    expeditionType = value;
                DataTable? expCompanyList = null;
                if (expeditionType == null)
                {
                    dictionary.TryGetValue("ExpCompanyList", out obj);
                    string expCompanyListJSON = Convert.ToString(obj);
                    expCompanyList = JsonConvert.DeserializeObject<DataTable>(expCompanyListJSON).DefaultView.ToTable(false, "ID");
                }
                var expeditionTypeList = await _expeditionService.GetExpeditionTypes(expeditionType, expCompanyList);
                _logger.LogInfo("Expedition Type Get");
                return Ok(expeditionTypeList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetEnvelopeMedia action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("GetExpCompanyTypes")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ExpeditionTypeViewModel>> GetExpCompanyTypes([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int value = 0;
                dictionary.TryGetValue("ExpCompanyID", out obj);
                int? expCompanyID = null;
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    expCompanyID = value;

                int? expeditionType = null;
                 if (expeditionType == null)
                {
                    dictionary.TryGetValue("ExpeditionType", out obj);
                    if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                        expeditionType = value;
                }
                var expeditionTypeList = await _expeditionService.GetExpCompanyTypes(expeditionType, expCompanyID);
                _logger.LogInfo("Expedition Type Get");
                return Ok(expeditionTypeList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetEnvelopeMedia action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("SetExpCompanyType")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ExpeditionTypeViewModel>> SetExpCompanyType([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int value = 0;
                dictionary.TryGetValue("ExpCompanyID", out obj);
                int expCompanyID = Int32.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("ExpeditionType", out obj);
                int expeditionType = Int32.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("RegistMode", out obj);
                bool registMode = bool.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("SeparationMode", out obj);
                bool separationMode = bool.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("BarcodeRegistMode", out obj);
                bool barcodeRegistMode = bool.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("ReturnAll", out obj);
                bool returnAll = bool.Parse(Convert.ToString(obj));

                var expeditionTypeList = await _expeditionService.SetExpCompanyType(expeditionType, expCompanyID, registMode, separationMode, barcodeRegistMode, returnAll);
                _logger.LogInfo("Expedition Type Get");
                return Ok(expeditionTypeList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetEnvelopeMedia action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("GetExpeditionZones")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ExpeditionZoneViewModel>> GetExpeditionZones([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ExpeditionZone", out obj);
                int value = 0;
                int? expeditionZone = null;
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    expeditionZone = value;
                DataTable? expCompanyList = null;
                if (expeditionZone == null)
                {
                    dictionary.TryGetValue("ExpCompanyList", out obj);
                    string expCompanyListJSON = Convert.ToString(obj);
                    expCompanyList = JsonConvert.DeserializeObject<DataTable>(expCompanyListJSON).DefaultView.ToTable(false, "ID");
                }
                var expeditionZoneList = await _expeditionService.GetExpeditionZones(expeditionZone, expCompanyList);
                _logger.LogInfo("Expedition Zone Get");
                return Ok(expeditionZoneList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetEnvelopeMedia action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }
       
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetExpeditionCompanies")]
        public async Task<ActionResult<List<dynamic>>> GetExpeditionCompanies([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ExpCompanyID", out obj);
                int value = 0;
                int? expCompanyID = null;
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    expCompanyID = value;
                DataTable expCompanyList = new DataTable();
                if (expCompanyID == null)
                {
                    dictionary.TryGetValue("ExpCompanyList", out obj);
                    string expCompanyListJSON = Convert.ToString(obj);
                    expCompanyList = JsonConvert.DeserializeObject<DataTable>(expCompanyListJSON).DefaultView.ToTable(false, "ID");
                }

                var expeditionCompaniesList = await _expeditionService.GetExpeditionCompanies(expCompanyID, expCompanyList);
                _logger.LogInfo("Expedition Companies Get");
                return Ok(expeditionCompaniesList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
                var expeditionCompanyConfigsList = await _expeditionService.GetExpeditionCompanyConfigs(exObjExpandoObject);
                _logger.LogInfo("Expedition Company Configs Get");
                return Ok(expeditionCompanyConfigsList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
                var expeditionCompanyConfigsList = await _expeditionService.GetExpeditionCompanyConfigCharacteristics(exObjExpandoObject);
                _logger.LogInfo("Expedition Company Config Characteristics Get");
                return Ok(expeditionCompanyConfigsList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
