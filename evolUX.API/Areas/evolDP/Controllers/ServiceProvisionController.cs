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
        public async Task<ActionResult<IEnumerable<ServiceTask>>> GetServiceTasks([FromBody] Dictionary<string, object> dictionary)
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

        //[HttpGet]
        //[ActionName("SetExpCompanyType")]
        ////[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<ActionResult<ResultsViewModel>> SetExpCompanyType([FromBody] Dictionary<string, object> dictionary)
        //{
        //    try
        //    {
        //        object obj;
        //        int value = 0;
        //        dictionary.TryGetValue("ExpCompanyID", out obj);
        //        int expCompanyID = Int32.Parse(Convert.ToString(obj));
        //        dictionary.TryGetValue("ExpeditionType", out obj);
        //        int expeditionType = Int32.Parse(Convert.ToString(obj));
        //        dictionary.TryGetValue("RegistMode", out obj);
        //        bool registMode = bool.Parse(Convert.ToString(obj));
        //        dictionary.TryGetValue("SeparationMode", out obj);
        //        bool separationMode = bool.Parse(Convert.ToString(obj));
        //        dictionary.TryGetValue("BarcodeRegistMode", out obj);
        //        bool barcodeRegistMode = bool.Parse(Convert.ToString(obj));

        //        ResultsViewModel viewmodel = new ResultsViewModel();
        //        viewmodel.Results = await _serviceProvision.SetExpCompanyType(expeditionType, expCompanyID, registMode, separationMode, barcodeRegistMode);
        //        _logger.LogInfo("Expedition Type Set");
        //        return Ok(viewmodel);
        //    }
        //    catch (SqlException ex)
        //    {
        //        return StatusCode(503, "Internal Server Error");
        //    }
        //    catch (Exception ex)
        //    {
        //        //log error
        //        _logger.LogError($"Something went wrong inside SetExpCompanyType action: {ex.Message}");
        //        return StatusCode(500, "Internal Server Erros");
        //    }
        //}


        //[HttpGet]
        //[ActionName("GetExpContracts")]
        ////[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<ActionResult<ExpeditionZoneViewModel>> GetExpContracts([FromBody] int expCompanyID)
        //{
        //    try
        //    {
        //        var expeditionRegistIDs = await _serviceProvision.GetExpContracts(expCompanyID);
        //        _logger.LogInfo("ExpContracts Get");
        //        return Ok(expeditionRegistIDs);
        //    }
        //    catch (SqlException ex)
        //    {
        //        return StatusCode(503, "Internal Server Error");
        //    }
        //    catch (Exception ex)
        //    {
        //        //log error
        //        _logger.LogError($"Something went wrong inside GetExpContracts action: {ex.Message}");
        //        return StatusCode(500, "Internal Server Erros");
        //    }
        //}

        //[HttpGet]
        //[ActionName("SetExpContract")]
        ////[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<ActionResult<ExpeditionZoneViewModel>> SetExpContract([FromBody] Dictionary<string, object> dictionary)
        //{
        //    try
        //    {
        //        object obj;
        //        dictionary.TryGetValue("ExpContract", out obj);
        //        string expContractJSON = Convert.ToString(obj);
        //        ExpContractElement expContract = JsonConvert.DeserializeObject<ExpContractElement>(expContractJSON);

        //        await _serviceProvision.SetExpContract(expContract);
        //        _logger.LogInfo("setExpContracts Get");
        //        return Ok();
        //    }
        //    catch (SqlException ex)
        //    {
        //        return StatusCode(503, "Internal Server Error");
        //    }
        //    catch (Exception ex)
        //    {
        //        //log error
        //        _logger.LogError($"Something went wrong inside GetExpContracts action: {ex.Message}");
        //        return StatusCode(500, "Internal Server Erros");
        //    }
        //}


        //[HttpGet]
        ////[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        //[ActionName("NewExpCompanyConfig")]
        //public async Task<ActionResult> NewExpCompanyConfig([FromBody] Dictionary<string, object> dictionary)
        //{
        //    try
        //    {
        //        object obj;
        //        dictionary.TryGetValue("ExpCompanyID", out obj);
        //        int expCompanyID = Int32.Parse(Convert.ToString(obj));

        //        dictionary.TryGetValue("StartDate", out obj);
        //        int startDate = Int32.Parse(Convert.ToString(obj));

        //        await _serviceProvision.NewExpCompanyConfig(expCompanyID, startDate);
        //        _logger.LogInfo("SetExpCompanyConfig Get");
        //        return Ok();
        //    }
        //    catch (SqlException ex)
        //    {
        //        return StatusCode(503, "Internal Server Error");
        //    }
        //    catch (Exception ex)
        //    {
        //        //log error
        //        _logger.LogError($"Something went wrong inside SetExpCompanyConfig action: {ex.Message}");
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

        //[HttpGet]
        ////[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        //[ActionName("GetExpCompanyConfigsResume")]
        //public async Task<ActionResult<IEnumerable<ExpCompanyConfigResume>>> GetExpCompanyConfigsResume([FromBody] Dictionary<string, object> dictionary)
        //{
        //    try
        //    {
        //        object obj;
        //        dictionary.TryGetValue("ExpCompanyID", out obj);
        //        int expCompanyID = Int32.Parse(Convert.ToString(obj));

        //        var result = await _serviceProvision.GetExpCompanyConfigsResume(expCompanyID);
        //        _logger.LogInfo("Expedition Company Configs Resume Get");
        //        return Ok(result);
        //    }
        //    catch (SqlException ex)
        //    {
        //        return StatusCode(503, "Internal Server Error");
        //    }
        //    catch (Exception ex)
        //    {
        //        //log error
        //        _logger.LogError($"Something went wrong inside GetExpCompanyConfigsResume action: {ex.Message}");
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

    }
}
