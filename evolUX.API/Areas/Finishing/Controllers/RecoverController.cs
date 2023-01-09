using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using Shared.Models.Areas.Finishing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using System.Data;
using Shared.BindingModels.Finishing;
using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.General;

namespace evolUX.API.Areas.Finishing.Controllers{
    [Route("api/finishing/[controller]/[action]")]
    [ApiController]
    public class RecoverController : ControllerBase
    {
        private readonly ILoggerService _logger;
        private readonly IRecoverRepository _recoverService;
        public RecoverController(IWrapperRepository repository, ILoggerService logger, IRecoverRepository recoverService)
        {
            _logger = logger;
            _recoverService = recoverService;
        }

        

        //THE SERVICECOMPANYLIST SHOULD USE A SESSION VARIABLE IN THE UI LAYER
        [HttpPost]
        [ActionName("RegistTotalRecover")]
        public async Task<ActionResult<ResultsViewModel>> RegistTotalRecover([FromBody] RegistPermissionLevel bindingModel)
        {
            try
            {
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _recoverService.RegistTotalRecover(bindingModel.FileBarcode, bindingModel.User, bindingModel.ServiceCompanyList, bindingModel.PermissionLevel);
                _logger.LogInfo("RegistTotalRecover Post");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside RegistTotalRecover Post action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [ActionName("RegistPartialRecover")]
        public async Task<ActionResult<ResultsViewModel>> RegistPartialRecover([FromBody] RegistElaboratePermissionLevel bindingModel)
        {
            try
            {
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _recoverService.RegistPartialRecover(bindingModel.StartBarcode, bindingModel.EndBarcode, bindingModel.User, bindingModel.ServiceCompanyList, bindingModel.PermissionLevel);
                _logger.LogInfo("RegistTotalRecover Post");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside RegistTotalRecover Post action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPost]
        [ActionName("RegistDetailRecover")]
        public async Task<ActionResult<ResultsViewModel>> RegistDetailRecover([FromBody] RegistElaboratePermissionLevel bindingModel)
        {
            try
            {
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _recoverService.RegistDetailRecover(bindingModel.StartBarcode, bindingModel.EndBarcode, bindingModel.User, bindingModel.ServiceCompanyList, bindingModel.PermissionLevel);
                _logger.LogInfo("RegistDetailRecover Post");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside RegistDetailRecover Post action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet]
        [ActionName("PendingRecoveries")]
        public async Task<ActionResult<ResultsViewModel>> GetPendingRecoveries([FromQuery] int ServiceCompanyID)
        {
            try
            {
                PendingRecoveriesViewModel viewmodel = new PendingRecoveriesViewModel();
                viewmodel.PendingRecoveries = await _recoverService.GetPendingRecoveries(ServiceCompanyID);
                _logger.LogInfo("PendingRecoveries Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside PendingRecoveries Get action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet]
        [ActionName("PendingRecoveriesRegistDetail")]
        public async Task<ActionResult<ResultsViewModel>> GetPendingRecoveriesRegistDetail([FromQuery] int ServiceCompanyID)
        {
            try
            {
                PendingRecoveriesViewModel viewmodel = new PendingRecoveriesViewModel();
                viewmodel.PendingRecoveries = await _recoverService.GetPendingRecoveriesRegistDetail(ServiceCompanyID);
                _logger.LogInfo("PendingRecoveriesRegistDetail Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside PendingRecoveriesRegistDetail Get action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
    }
}
