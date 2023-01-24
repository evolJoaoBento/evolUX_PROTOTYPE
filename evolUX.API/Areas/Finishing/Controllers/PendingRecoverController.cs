using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using System.Data;
using Newtonsoft.Json;
using evolUX.API.Areas.Finishing.Services;
using Shared.Models.General;
using Shared.ViewModels.General;

namespace evolUX.API.Areas.Finishing.Controllers
{
    [Route("api/finishing/[controller]/[action]")]
    [ApiController]
    public class PendingRecoverController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IPendingRecoverService _pendingRecoverService;
        public PendingRecoverController(IWrapperRepository repository, ILoggerService logger, IPendingRecoverService pendingRecoverService)
        {
            _repository = repository;
            _logger = logger;
            _pendingRecoverService = pendingRecoverService;
        }


        //THE SERVICECOMPANYLIST SHOULD USE A SESSION VARIABLE IN THE UI LAYER
        [HttpGet]
        [ActionName("GetServiceCompanies")]
        public async Task<ActionResult<ServiceCompanyViewModel>> GetServiceCompanies([FromBody] string ServiceCompanyListJSON)
        {
            DataTable ServiceCompanyList = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyListJSON);
            try
            {
                ServiceCompanyViewModel viewmodel = new ServiceCompanyViewModel();
                viewmodel.ServiceCompanies = await _pendingRecoverService.GetServiceCompanies(ServiceCompanyList);
                _logger.LogInfo("GetServiceCompanies Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DocCode action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("GetPendingRecoveries")]
        public async Task<ActionResult<PendingRecoverDetailViewModel>> GetPendingRecoveries([FromQuery] int ServiceCompanyID, [FromQuery] string serviceCompanyCode)
        {
            try
            {
                PendingRecoverDetailViewModel viewmodel = await _pendingRecoverService.GetPendingRecoveries(ServiceCompanyID, serviceCompanyCode);
                _logger.LogInfo("GetPendingRecoveries Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DocCode action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("RegistPendingRecover")]
        public async Task<ActionResult<ResultsViewModel>> RegistPendingRecover([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("UserID", out obj);
                int UserID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("RecoverType", out obj);
                string RecoverType = Convert.ToString(obj);
                dictionary.TryGetValue("ServiceCompanyCode", out obj);
                string ServiceCompanyCode = Convert.ToString(obj);
                dictionary.TryGetValue("ServiceCompanyID", out obj);
                int ServiceCompanyID = Convert.ToInt32(obj.ToString());

                Result viewmodel = await _pendingRecoverService.RegistPendingRecover(ServiceCompanyID, ServiceCompanyCode, RecoverType, UserID);
                _logger.LogInfo("RegistPendingRecover Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get Printers action: {ex.Message}");
                //return StatusCode(500, "Internal Server Error");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }
    }
}
