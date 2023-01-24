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
using evolUX.API.Areas.Finishing.Services;
using Newtonsoft.Json;

namespace evolUX.API.Areas.Finishing.Controllers{
    [Route("api/finishing/[controller]/[action]")]
    [ApiController]
    public class PendingRegistController : ControllerBase
    {
        private readonly ILoggerService _logger;
        private readonly IPendingRegistService _pendingRegistService;
        private readonly IConcludedPrintService _concludedPrintService;
        private readonly IConcludedFullfillService _concludedFullfillService;
        private readonly IRecoverService _recoverService;
        public PendingRegistController(IWrapperRepository repository, ILoggerService logger, IPendingRegistService pendingRegistService, IConcludedPrintService concludedPrintService, IConcludedFullfillService concludedFullfillService, IRecoverService recoverService)
        {
            _logger = logger;
            _pendingRegistService = pendingRegistService;
            _concludedPrintService = concludedPrintService;
            _concludedFullfillService = concludedFullfillService;
            _recoverService = recoverService;
        }



        [HttpGet]
        [ActionName("GetPendingRegist")]
        public async Task<ActionResult<PendingRegistViewModel>> GetPendingRegist([FromBody] string ServiceCompanyListJSON)
        {
            DataTable ServiceCompanyList = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyListJSON);
            try
            {
                PendingRegistViewModel viewmodel = await _pendingRegistService.GetPendingRegist(ServiceCompanyList);
                _logger.LogInfo("PendingRegist Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get PendingRegist action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("GetPendingRegistDetail")]
        public async Task<ActionResult<PendingRegistViewModel>> GetPendingRegistDetail([FromQuery] int RunID, [FromBody] string ServiceCompanyListJSON)
        {
            DataTable ServiceCompanyList = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyListJSON);
            try
            {
                PendingRegistDetailViewModel viewmodel = await _pendingRegistService.GetPendingRegistDetail(RunID, ServiceCompanyList);
                _logger.LogInfo("PendingRegistDetail Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get PendingRegistDetail action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("RegistPrint")]
        public async Task<ActionResult<ResultsViewModel>> RegistPrint([FromBody] Regist bindingModel)
        {
            try
            {
                DataTable ServiceCompanyList = JsonConvert.DeserializeObject<DataTable>(bindingModel.ServiceCompanyList);
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _concludedPrintService.RegistPrint(bindingModel.FileBarcode, bindingModel.User, ServiceCompanyList);
                _logger.LogInfo("RegistPrint Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside RegistPrint Post action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("RegistFullFill")]
        public async Task<ActionResult<ResultsViewModel>> RegistFullFill([FromBody] Regist bindingModel)
        {
            try
            {
                DataTable ServiceCompanyList = JsonConvert.DeserializeObject<DataTable>(bindingModel.ServiceCompanyList);
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _concludedFullfillService.RegistFullFill(bindingModel.FileBarcode, bindingModel.User, ServiceCompanyList);
                _logger.LogInfo("RegistFullFill Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside RegistFullFill Post action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("RegistPartialRecover")]
        public async Task<ActionResult<ResultsViewModel>> RegistPartialRecover([FromBody] RegistElaborate bindingModel)
        {
            try
            {
                DataTable ServiceCompanyList = JsonConvert.DeserializeObject<DataTable>(bindingModel.ServiceCompanyList);
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _recoverService.RegistPartialRecover(bindingModel.StartBarcode, bindingModel.EndBarcode, bindingModel.User, ServiceCompanyList, bindingModel.PermissionLevel);
                _logger.LogInfo("RegistTotalRecover Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside RegistTotalRecover Post action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("RegistTotalRecover")]
        public async Task<ActionResult<ResultsViewModel>> RegistTotalRecover([FromBody] Regist bindingModel)
        {
            try
            {
                DataTable ServiceCompanyList = JsonConvert.DeserializeObject<DataTable>(bindingModel.ServiceCompanyList);
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _recoverService.RegistTotalRecover(bindingModel.FileBarcode, bindingModel.User, ServiceCompanyList, bindingModel.PermissionLevel);
                _logger.LogInfo("RegistTotalRecover Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside RegistTotalRecover Post action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("RegistDetailRecover")]
        public async Task<ActionResult<ResultsViewModel>> RegistDetailRecover([FromBody] RegistElaborate bindingModel)
        {
            try
            {
                DataTable ServiceCompanyList = JsonConvert.DeserializeObject<DataTable>(bindingModel.ServiceCompanyList);
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _recoverService.RegistDetailRecover(bindingModel.StartBarcode, bindingModel.EndBarcode, bindingModel.User, ServiceCompanyList, bindingModel.PermissionLevel);
                _logger.LogInfo("RegistDetailRecover Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside RegistDetailRecover Post action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        //[HttpGet]
        //[ActionName("PendingRecoveries")]
        //public async Task<ActionResult<ResultsViewModel>> GetPendingRecoveries([FromQuery] int ServiceCompanyID)
        //{
        //    try
        //    {
        //        PendingRecoveriesViewModel viewmodel = new PendingRecoveriesViewModel();
        //        viewmodel.PendingRecoveries = await _recoverService.GetPendingRecoveries(ServiceCompanyID);
        //        _logger.LogInfo("PendingRecoveries Get");
        //        return Ok(viewmodel);
        //    }
        //    catch (Exception ex)
        //    {
        //        //log error
        //        _logger.LogError($"Something went wrong inside PendingRecoveries Get action: {ex.Message}");
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

        //[HttpGet]
        //[ActionName("PendingRecoveriesRegistDetail")]
        //public async Task<ActionResult<ResultsViewModel>> GetPendingRecoveriesRegistDetail([FromQuery] int ServiceCompanyID)
        //{
        //    try
        //    {
        //        PendingRecoveriesViewModel viewmodel = new PendingRecoveriesViewModel();
        //        viewmodel.PendingRecoveries = await _recoverService.GetPendingRecoveriesRegistDetail(ServiceCompanyID);
        //        _logger.LogInfo("PendingRecoveriesRegistDetail Get");
        //        return Ok(viewmodel);
        //    }
        //    catch (Exception ex)
        //    {
        //        //log error
        //        _logger.LogError($"Something went wrong inside PendingRecoveriesRegistDetail Get action: {ex.Message}");
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}
    }
}
