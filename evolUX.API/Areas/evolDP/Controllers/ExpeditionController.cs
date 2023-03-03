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
using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.General;
using System.Reflection.Emit;

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
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetExpeditionCompanies")]
        public async Task<ActionResult<ExpeditionTypeViewModel>> GetExpeditionCompanies([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                ExpeditionTypeViewModel expeditionTypeList = new ExpeditionTypeViewModel();
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
                    expeditionTypeList = await _expeditionService.GetExpeditionCompanies(expCompanyList);
                }
                else
                {
                    expeditionTypeList.ExpCompanies = await _expeditionService.GetExpeditionCompanies(expCompanyID, expCompanyList);
                }
                _logger.LogInfo("Expedition Companies Get");
                return Ok(expeditionTypeList);
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
                dictionary.TryGetValue("ExpCompanyList", out obj);
                string expCompanyListJSON = Convert.ToString(obj);
                if (!string.IsNullOrEmpty(expCompanyListJSON))
                    expCompanyList = JsonConvert.DeserializeObject<DataTable>(expCompanyListJSON).DefaultView.ToTable(false, "ID");
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
                _logger.LogError($"Something went wrong inside GetExpeditionTypes action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("GetExpCompanyTypes")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<ExpCompanyType>>> GetExpCompanyTypes([FromBody] Dictionary<string, object> dictionary)
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
                _logger.LogInfo("Expedition Company Type Get");
                return Ok(expeditionTypeList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetExpCompanyTypes action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("SetExpCompanyType")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ResultsViewModel>> SetExpCompanyType([FromBody] Dictionary<string, object> dictionary)
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

                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _expeditionService.SetExpCompanyType(expeditionType, expCompanyID, registMode, separationMode, barcodeRegistMode);
                _logger.LogInfo("Expedition Type Set");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside SetExpCompanyType action: {ex.Message}");
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
                _logger.LogError($"Something went wrong inside GetExpeditionZones action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("GetExpeditionRegistIDs")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ExpeditionZoneViewModel>> GetExpeditionRegistIDs([FromBody] int expCompanyID)
        {
            try
            {
                var expeditionRegistIDs = await _expeditionService.GetExpeditionRegistIDs(expCompanyID);
                _logger.LogInfo("ExpeditionRegistIDs Get");
                return Ok(expeditionRegistIDs);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetExpeditionRegistIDs action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("SetExpeditionRegistID")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SetExpeditionRegistID([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ExpRegist", out obj);
                string expRegistJSON = Convert.ToString(obj);
                ExpeditionRegistElement expRegist = JsonConvert.DeserializeObject<ExpeditionRegistElement>(expRegistJSON);

                await _expeditionService.SetExpeditionRegistID(expRegist);
                _logger.LogInfo("SetExpeditionRegistIDs Get");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetExpeditionRegistIDs action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("GetExpContracts")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ExpeditionZoneViewModel>> GetExpContracts([FromBody] int expCompanyID)
        {
            try
            {
                var expeditionRegistIDs = await _expeditionService.GetExpContracts(expCompanyID);
                _logger.LogInfo("ExpContracts Get");
                return Ok(expeditionRegistIDs);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetExpContracts action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("SetExpContract")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ExpeditionZoneViewModel>> SetExpContract([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ExpContract", out obj);
                string expContractJSON = Convert.ToString(obj);
                ExpContractElement expContract = JsonConvert.DeserializeObject<ExpContractElement>(expContractJSON);

                await _expeditionService.SetExpContract(expContract);
                _logger.LogInfo("setExpContracts Get");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetExpContracts action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        //TODO: UNTESTED
        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetExpCompanyConfigs")]
        public async Task<ActionResult<IEnumerable<ExpCompanyConfig>>> GetExpCompanyConfigs([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ExpCompanyID", out obj);
                int expCompanyID = Int32.Parse(Convert.ToString(obj));
                int expeditionType = 0;
                int expeditionZone = 0;
                int startDate = 0;
                int value = 0;
                dictionary.TryGetValue("ExpeditionType", out obj);
                string str = Convert.ToString(obj).ToString();
                if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out value))
                    expeditionType = value;
                dictionary.TryGetValue("ExpeditionZone", out obj);
                str = Convert.ToString(obj).ToString();
                if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out value))
                    expeditionZone = value;
                dictionary.TryGetValue("StartDate", out obj);
                str = Convert.ToString(obj).ToString();
                if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out value))
                    startDate = value;

                var result = await _expeditionService.GetExpCompanyConfigs(expCompanyID, startDate, expeditionType, expeditionZone);
                _logger.LogInfo("Expedition Company Configs Get");
                return Ok(result);
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

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("SetExpCompanyConfig")]
        public async Task<ActionResult> SetExpCompanyConfig([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ExpCompanyConfig", out obj);
                string expCompanyConfigJSON = Convert.ToString(obj);
                ExpCompanyConfig expConfig = JsonConvert.DeserializeObject<ExpCompanyConfig>(expCompanyConfigJSON);

                await _expeditionService.SetExpCompanyConfig(expConfig);
                _logger.LogInfo("SetExpCompanyConfig Get");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside SetExpCompanyConfig action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetExpCompanyConfigsResume")]
        public async Task<ActionResult<IEnumerable<ExpCompanyConfigResume>>> GetExpCompanyConfigsResume([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ExpCompanyID", out obj);
                int expCompanyID = Int32.Parse(Convert.ToString(obj));

                var result = await _expeditionService.GetExpCompanyConfigsResume(expCompanyID);
                _logger.LogInfo("Expedition Company Configs Resume Get");
                return Ok(result);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetExpCompanyConfigsResume action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
