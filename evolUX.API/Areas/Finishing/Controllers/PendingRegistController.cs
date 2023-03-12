using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using Shared.Models.Areas.Finishing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using System.Data;
using Shared.BindingModels.Finishing;
using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.General;
using evolUX.API.Areas.Finishing.Services;
using Newtonsoft.Json;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using System.Data.SqlClient;

namespace evolUX.API.Areas.Finishing.Controllers
{
    [Route("api/finishing/[controller]/[action]")]
    [ApiController]
    public class PendingRegistController : ControllerBase
    {
        private readonly ILoggerService _logger;
        private readonly IPendingRegistService _pendingRegistService;
        private readonly IConcludedPrintingService _concludedPrintService;
        private readonly IConcludedFulfilmentService _ConcludedFulfilmentService;
        private readonly IRecoverService _recoverService;
        public PendingRegistController(IWrapperRepository repository, ILoggerService logger, IPendingRegistService pendingRegistService, IConcludedPrintingService concludedPrintService, IConcludedFulfilmentService ConcludedFulfilmentService, IRecoverService recoverService)
        {
            _logger = logger;
            _pendingRegistService = pendingRegistService;
            _concludedPrintService = concludedPrintService;
            _ConcludedFulfilmentService = ConcludedFulfilmentService;
            _recoverService = recoverService;
        }



        [HttpGet]
        [ActionName("GetPendingRegist")]
        public async Task<ActionResult<PendingRegistViewModel>> GetPendingRegist([FromBody] string ServiceCompanyListJSON)
        {
            DataTable ServiceCompanies = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyListJSON);
            DataTable ServiceCompanyList = ServiceCompanies.DefaultView.ToTable(false, "ID");
            try
            {
                PendingRegistViewModel viewmodel = await _pendingRegistService.GetPendingRegist(ServiceCompanyList);
                _logger.LogInfo("PendingRegist Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
            DataTable ServiceCompanies = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyListJSON);
            DataTable ServiceCompanyList = ServiceCompanies.DefaultView.ToTable(false, "ID");
            try
            {
                PendingRegistDetailViewModel viewmodel = await _pendingRegistService.GetPendingRegistDetail(RunID, ServiceCompanyList);
                _logger.LogInfo("PendingRegistDetail Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
                DataTable ServiceCompanies = JsonConvert.DeserializeObject<DataTable>(bindingModel.ServiceCompanyList);
                DataTable ServiceCompanyList = ServiceCompanies.DefaultView.ToTable(false, "ID");
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _concludedPrintService.RegistPrint(bindingModel.FileBarcode, bindingModel.User, ServiceCompanyList);
                _logger.LogInfo("RegistPrint Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
                DataTable ServiceCompanies = JsonConvert.DeserializeObject<DataTable>(bindingModel.ServiceCompanyList);
                DataTable ServiceCompanyList = ServiceCompanies.DefaultView.ToTable(false, "ID");
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _ConcludedFulfilmentService.RegistFullFill(bindingModel.FileBarcode, bindingModel.User, ServiceCompanyList);
                _logger.LogInfo("RegistFullFill Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
                DataTable ServiceCompanies = JsonConvert.DeserializeObject<DataTable>(bindingModel.ServiceCompanyList);
                DataTable ServiceCompanyList = ServiceCompanies.DefaultView.ToTable(false, "ID");
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _recoverService.RegistPartialRecover(bindingModel.StartBarcode, bindingModel.EndBarcode, bindingModel.User, ServiceCompanyList, bindingModel.PermissionLevel);
                _logger.LogInfo("RegistTotalRecover Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
                DataTable ServiceCompanies = JsonConvert.DeserializeObject<DataTable>(bindingModel.ServiceCompanyList);
                DataTable ServiceCompanyList = ServiceCompanies.DefaultView.ToTable(false, "ID");
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _recoverService.RegistTotalRecover(bindingModel.FileBarcode, bindingModel.User, ServiceCompanyList, bindingModel.PermissionLevel);
                _logger.LogInfo("RegistTotalRecover Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
                DataTable ServiceCompanies = JsonConvert.DeserializeObject<DataTable>(bindingModel.ServiceCompanyList);
                DataTable ServiceCompanyList = ServiceCompanies.DefaultView.ToTable(false, "ID");
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _recoverService.RegistDetailRecover(bindingModel.StartBarcode, bindingModel.EndBarcode, bindingModel.User, ServiceCompanyList, bindingModel.PermissionLevel);
                _logger.LogInfo("RegistDetailRecover Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside RegistDetailRecover Post action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
