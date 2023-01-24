using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using System.Data;
using Newtonsoft.Json;

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
        public async Task<ActionResult<PendingRecoverDetailViewModel>> GetPendingRecoveries([FromQuery] int ServiceCompanyID)
        {
            try
            {
                PendingRecoverDetailViewModel viewmodel = await _pendingRecoverService.GetPendingRecoveries(ServiceCompanyID);
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
    }
}
