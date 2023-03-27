using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using System.Data.SqlClient;
using evolUX.API.Areas.evolDP.Services;
using Newtonsoft.Json;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using Shared.ViewModels.General;
using System.Data;
using System.Collections.Generic;
using evolUX.API.Models;
using System.Reflection.Emit;

namespace evolUX.API.Areas.evolDP.Controllers
{
    [Route("api/evoldp/[controller]/[action]")]
    [ApiController]
    public class ServiceProvisionController : Controller
    {
        private readonly ILoggerService _logger;
        private readonly IServiceProvisionService _serviceProvision;

        public ServiceProvisionController(ILoggerService logger, IServiceProvisionService serviceProvision)
        {
            _logger = logger;
            _serviceProvision = serviceProvision;
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetServiceCompanies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetServiceCompanies([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                IEnumerable<Company> serviceCompanies = new List<Company>();
                object obj;
                dictionary.TryGetValue("ServiceCompanyID", out obj);
                int value = 0;
                int? serviceCompanyID = null;
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    serviceCompanyID = value;
                DataTable serviceCompanyList = new DataTable();
                if (serviceCompanyID == null)
                {
                    dictionary.TryGetValue("ServiceCompanyList", out obj);
                    string serviceCompanyListJSON = Convert.ToString(obj);
                    serviceCompanyList = JsonConvert.DeserializeObject<DataTable>(serviceCompanyListJSON).DefaultView.ToTable(false, "ID");
                    serviceCompanies = await _serviceProvision.GetServiceCompanies(serviceCompanyList);
                }
                else
                {
                    serviceCompanies = await _serviceProvision.GetServiceCompanies((int)serviceCompanyID);
                }

                _logger.LogInfo("Service Companies Get");
                return Ok(serviceCompanies);
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
        [ActionName("GetServiceCompanyRestrictions")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<ServiceCompanyRestriction>>> GetServiceCompanyRestrictions([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ServiceCompanyID", out obj);
                int value = 0;
                int? serviceCompanyID = null;
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    serviceCompanyID = value;
                var restrictions = await _serviceProvision.GetServiceCompanyRestrictions(serviceCompanyID);
                _logger.LogInfo("ServiceCompany Restrictions  Get");
                return Ok(restrictions);
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
        [ActionName("SetServiceCompanyRestriction")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SetServiceCompanyRestriction([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ServiceCompanyID", out obj);
                int serviceCompanyID = Int32.Parse(Convert.ToString(obj));

                dictionary.TryGetValue("MaterialTypeID", out obj);
                int materialTypeID = Int32.Parse(Convert.ToString(obj));

                dictionary.TryGetValue("MaterialPosition", out obj);
                int materialPosition = Int32.Parse(Convert.ToString(obj));

                int value = 0;
                int fileSheetsCutoffLevel = 0;
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    fileSheetsCutoffLevel = value;

                dictionary.TryGetValue("RestrictionMode", out obj);
                bool restrictionMode = bool.Parse(Convert.ToString(obj));

                await _serviceProvision.SetServiceCompanyRestriction(serviceCompanyID, materialTypeID, materialPosition, fileSheetsCutoffLevel, restrictionMode);
                _logger.LogInfo("ServiceCompany Restrictions  Get");
                return Ok();
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
        [ActionName("GetServiceCompanyConfigsResume")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<ServiceCompanyServiceResume>>> GetServiceCompanyConfigsResume([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int value = 0;
                int? serviceCompanyID = null;
                int? serviceTypeID = null;
                int? serviceID = null;
                int? costDate = null;
                dictionary.TryGetValue("ServiceCompanyID", out obj);
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    serviceCompanyID = value;
                dictionary.TryGetValue("ServiceTypID", out obj);
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    serviceTypeID = value;
                dictionary.TryGetValue("ServiceID", out obj);
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    serviceID = value;
                dictionary.TryGetValue("CostDate", out obj);
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    costDate = value;

                var configs = await _serviceProvision.GetServiceCompanyConfigsResume(serviceCompanyID, serviceTypeID, serviceID, costDate);
                _logger.LogInfo("ServiceCompany Configs Resume Get");
                return Ok(configs);
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
        [ActionName("GetServiceCompanyList")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<int>>> GetServiceCompanyList([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int value = 0;
                int? serviceCompanyID = null;
                int? serviceTypeID = null;
                int? serviceID = null;
                int? costDate = null;
                dictionary.TryGetValue("ServiceCompanyID", out obj);
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    serviceCompanyID = value;
                dictionary.TryGetValue("ServiceTypID", out obj);
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    serviceTypeID = value;
                dictionary.TryGetValue("ServiceID", out obj);
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    serviceID = value;
                dictionary.TryGetValue("CostDate", out obj);
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    costDate = value;

                var companyList = await _serviceProvision.GetServiceCompanyList(serviceCompanyID, serviceTypeID, serviceID, costDate);
                _logger.LogInfo("ServiceCompanyList Get");
                return Ok(companyList);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetServiceCompanyList action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }
        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetServiceCompanyConfigs")]
        public async Task<ActionResult<IEnumerable<ServiceCompanyService>>> GetServiceCompanyConfigs([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ServiceCompanyID", out obj);
                int serviceCompanyID = Int32.Parse(Convert.ToString(obj));
                int serviceTypeID = 0;
                int serviceID = 0;
                int costDate = 0;
                int value = 0;
                dictionary.TryGetValue("ServiceTypeID", out obj);
                string str = Convert.ToString(obj).ToString();
                if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out value))
                    serviceTypeID = value;
                dictionary.TryGetValue("ServiceID", out obj);
                str = Convert.ToString(obj).ToString();
                if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out value))
                    serviceID = value;
                dictionary.TryGetValue("CostDate", out obj);
                str = Convert.ToString(obj).ToString();
                if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out value))
                    costDate = value;

                var result = await _serviceProvision.GetServiceCompanyConfigs(serviceCompanyID, costDate, serviceTypeID, serviceID);
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
        [ActionName("SetServiceCompanyConfig")]
        public async Task<ActionResult> SetServiceCompanyConfig([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ServiceCompanyConfig", out obj);
                string serviceCompanyConfigJSON = Convert.ToString(obj);
                ServiceCompanyService serviceConfig = JsonConvert.DeserializeObject<ServiceCompanyService>(serviceCompanyConfigJSON);

                await _serviceProvision.SetServiceCompanyConfig(serviceConfig);
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
        [ActionName("GetServices")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<ServiceElement>>> GetServices([FromBody] int serviceTypeID)
        {
            try
            {
                var expeditionRegistIDs = await _serviceProvision.GetServices(serviceTypeID);
                _logger.LogInfo("GetServices Get");
                return Ok(expeditionRegistIDs);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetServices action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("SetService")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SetService([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("Service", out obj);
                string serviceJSON = Convert.ToString(obj);
                ServiceElement service = JsonConvert.DeserializeObject<ServiceElement>(serviceJSON);

                await _serviceProvision.SetService(service);
                _logger.LogInfo("SetService Get");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside SetService action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("GetAvailableServiceTypes")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<ServiceTypeElement>>> GetAvailableServiceTypes()
        {
            try
            {
                IEnumerable<ServiceTypeElement> result = await _serviceProvision.GetAvailableServiceTypes();
                _logger.LogInfo("GetAvailableServiceTypes Get");
                return Ok(result);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetAvailableServiceTypes action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("GetServiceTypes")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ServiceTypeViewModel>> GetServiceTypes()
        {
            try
            {
                ServiceTypeViewModel viewModel = await _serviceProvision.GetServiceTypes(null);
                _logger.LogInfo("GetServiceTypes Get");
                return Ok(viewModel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetServiceTypes action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("SetServiceType")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SetServiceType([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ServiceTypeID", out obj);
                int value = 0;
                int serviceTypeID = 0;
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    serviceTypeID = value;
                dictionary.TryGetValue("ServiceTypeCode", out obj);
                string serviceTypeCode = Convert.ToString(obj);
                dictionary.TryGetValue("ServiceTypeDesc", out obj);
                string serviceTypeDesc = Convert.ToString(obj);

                await _serviceProvision.SetServiceType(serviceTypeID, serviceTypeCode, serviceTypeDesc);
                _logger.LogInfo("SetServiceType Get");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside SetServiceType action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("GetServiceTasks")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<ServiceTaskElement>>> GetServiceTasks([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ServiceTaskID", out obj);
                int? serviceTaskID = null;
                int value = 0;
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value) && value > 0)
                    serviceTaskID = value;

                var serviceTasks = await _serviceProvision.GetServiceTasks(serviceTaskID);
                _logger.LogInfo("GetServiceTasks Get");
                return Ok(serviceTasks);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetServiceTasks action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        [ActionName("SetServiceTask")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SetServiceTask([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int value = 0;
                dictionary.TryGetValue("ServiceTaskID", out obj);
                int serviceTaskID = Int32.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("ServiceTaskCode", out obj);
                string serviceTaskCode = Convert.ToString(obj);
                dictionary.TryGetValue("ServiceTaskDesc", out obj);
                string serviceTaskDesc = Convert.ToString(obj);

                dictionary.TryGetValue("RefServiceTaskID", out obj);
                int refServiceTaskID = Int32.Parse(Convert.ToString(obj));

                int complementServiceTaskID = 0;
                int externalExpeditionMode = 0;
                string stationExceededDesc = "";
                if (refServiceTaskID == 0)
                {
                    dictionary.TryGetValue("ComplementServiceTaskID", out obj);
                    complementServiceTaskID = Int32.Parse(Convert.ToString(obj));
                    dictionary.TryGetValue("ExternalExpeditionMode", out obj);
                    externalExpeditionMode = Int32.Parse(Convert.ToString(obj));
                    dictionary.TryGetValue("StationExceededDesc", out obj);
                    stationExceededDesc = Convert.ToString(obj);
                }
                await _serviceProvision.SetServiceTask(serviceTaskID, serviceTaskCode, serviceTaskDesc, refServiceTaskID, complementServiceTaskID, externalExpeditionMode, stationExceededDesc);
                _logger.LogInfo("SetServiceTask Set");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside SetServiceTask action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetExpCodes")]
        public async Task<ActionResult<IEnumerable<ExpCodeElement>>> GetExpCodes([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                int serviceTaskID = 0;
                int expCompanyID = 0;
                string expCode = "";
                object obj;
                dictionary.TryGetValue("ServiceTaskID", out obj);
                if (obj!= null)
                    serviceTaskID = Int32.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("ExpCompanyID", out obj);
                if (obj != null)
                    expCompanyID = Int32.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("ExpCode", out obj);
                if (obj != null)
                    expCode = Convert.ToString(obj);

                var result = await _serviceProvision.GetExpCodes(serviceTaskID, expCompanyID, expCode);
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

        [HttpGet]
        [ActionName("DeleteServiceType")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteServiceType([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int value = 0;
                dictionary.TryGetValue("ServiceTaskID", out obj);
                int serviceTaskID = Int32.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("ServiceTypeID", out obj);
                int serviceTypeID = Int32.Parse(Convert.ToString(obj));

                await _serviceProvision.DeleteServiceType(serviceTaskID, serviceTypeID);
                _logger.LogInfo("DeleteServiceType Set");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside DeleteServiceType action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }
        
        [HttpGet]
        [ActionName("AddServiceType")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> AddServiceType([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int value = 0;
                dictionary.TryGetValue("ServiceTaskID", out obj);
                int serviceTaskID = Int32.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("ServiceTypeID", out obj);
                int serviceTypeID = Int32.Parse(Convert.ToString(obj));
 
                await _serviceProvision.AddServiceType(serviceTaskID, serviceTypeID);
                _logger.LogInfo("AddServiceType Set");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside AddServiceType action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetExpCenters")]
        public async Task<ActionResult<IEnumerable<ExpCenterElement>>> GetExpCenters([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ExpCode", out obj);
                string expCode = Convert.ToString(obj);
                dictionary.TryGetValue("ServiceCompanyList", out obj);
                string serviceCompanyListJSON = Convert.ToString(obj);
                DataTable serviceCompanyList = JsonConvert.DeserializeObject<DataTable>(serviceCompanyListJSON).DefaultView.ToTable(false, "ID");

                var result = await _serviceProvision.GetExpCenters(expCode, serviceCompanyList);

                return Ok(result);
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
        [ActionName("SetExpCenter")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SetExpCenter([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int value = 0;
                dictionary.TryGetValue("ExpCode", out obj);
                string expCode = Convert.ToString(obj);
                dictionary.TryGetValue("ExpCenterCode", out obj);
                string expCenterCode = Convert.ToString(obj);
                dictionary.TryGetValue("ServiceCompanyID", out obj);
                int serviceCompanyID = Int32.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("ExpeditionZone", out obj);
                string expeditionZone = Convert.ToString(obj);
                dictionary.TryGetValue("Description1", out obj);
                string description1 = Convert.ToString(obj);
                dictionary.TryGetValue("Description2", out obj);
                string description2 = Convert.ToString(obj);
                dictionary.TryGetValue("Description3", out obj);
                string description3 = Convert.ToString(obj);

                await _serviceProvision.SetExpCenter(expCode, expCenterCode, description1, description2, description3, serviceCompanyID, expeditionZone);
                _logger.LogInfo("SetExpCenter Set");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside SetExpCenter action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }
        [HttpGet]
        [ActionName("GetServiceCompanyExpCodeConfigs")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<ServiceCompanyExpCodeConfig>>> GetServiceCompanyExpCodeConfigs([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int value = 0;
                dictionary.TryGetValue("ExpCode", out obj);
                string expCode = Convert.ToString(obj);
                dictionary.TryGetValue("ExpCenterCode", out obj);
                string expCenterCode = Convert.ToString(obj);
                dictionary.TryGetValue("ServiceCompanyID", out obj);
                int serviceCompanyID = Int32.Parse(Convert.ToString(obj));

                var result = await _serviceProvision.GetServiceCompanyExpCodeConfigs(expCode, serviceCompanyID, expCenterCode);
                _logger.LogInfo("GetServiceCompanyExpCodeConfigsGet");
                return Ok(result);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetServiceCompanyExpCodeConfigs action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }
        [HttpGet]
        [ActionName("DeleteServiceCompanyExpCodeConfig")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteServiceCompanyExpCodeConfig([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int value = 0;
                dictionary.TryGetValue("ExpCode", out obj);
                string expCode = Convert.ToString(obj);
                dictionary.TryGetValue("ExpCenterCode", out obj);
                string expCenterCode = Convert.ToString(obj);
                dictionary.TryGetValue("ServiceCompanyID", out obj);
                int serviceCompanyID = Int32.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("ExpLevel", out obj);
                int expLevel = Int32.Parse(Convert.ToString(obj));

                await _serviceProvision.DeleteServiceCompanyExpCodeConfig(expCode, serviceCompanyID, expCenterCode, expLevel);
                _logger.LogInfo("SetServiceCompanyExpCodeConfig");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside SetServiceCompanyExpCodeConfig action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }
        [HttpGet]
        [ActionName("SetServiceCompanyExpCodeConfig")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SetServiceCompanyExpCodeConfig([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                int value = 0;
                dictionary.TryGetValue("ExpCode", out obj);
                string expCode = Convert.ToString(obj);
                dictionary.TryGetValue("ExpCenterCode", out obj);
                string expCenterCode = Convert.ToString(obj);
                dictionary.TryGetValue("ServiceCompanyID", out obj);
                int serviceCompanyID = Int32.Parse(Convert.ToString(obj));
                dictionary.TryGetValue("ExpLevel", out obj);
                int expLevel = Int32.Parse(Convert.ToString(obj));
                string fullFillMaterialCode = Convert.ToString(obj);
                dictionary.TryGetValue("FullFillMaterialCode", out obj);

                int docMaxSheets = 0;
                dictionary.TryGetValue("DocMaxSheets", out obj);
                if (obj != null)
                    docMaxSheets = Int32.Parse(Convert.ToString(obj));

                string barcode = "";
                dictionary.TryGetValue("Barcode", out obj);
                if (obj != null)
                    barcode = Convert.ToString(obj);

                await _serviceProvision.SetServiceCompanyExpCodeConfig(expCode, serviceCompanyID, expCenterCode, expLevel, fullFillMaterialCode, docMaxSheets, barcode);
                _logger.LogInfo("SetServiceCompanyExpCodeConfig");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside SetServiceCompanyExpCodeConfig action: {ex.Message}");
                return StatusCode(500, "Internal Server Erros");
            }
        }
    }
}
