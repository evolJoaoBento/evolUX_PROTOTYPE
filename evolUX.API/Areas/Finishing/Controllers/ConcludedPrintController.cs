using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
//using Shared.Models.Areas.Finishing;
using Shared.ViewModels.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using Shared.Models.Areas.Finishing;
using System.Data;
using Shared.BindingModels.Finishing;

namespace evolUX.API.Areas.Finishing.Controllers
{
    [Route("api/finishing/[controller]/[action]")]
    [ApiController]
    public class ConcludedPrintController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IPrintedFilesRepository _concludedPrintService;
        public ConcludedPrintController(IWrapperRepository repository, ILoggerService logger, IPrintedFilesRepository concludedPrintService)
        {
            _repository = repository;
            _logger = logger;
            _concludedPrintService = concludedPrintService;
        }


        //THE SERVICECOMPANYLIST SHOULD USE A SESSION VARIABLE IN THE UI LAYER
        [HttpPost]
        [ActionName("RegistPrint")]
        public async Task<ActionResult<ResultsViewModel>> RegistPrint([FromBody] Regist bindingModel)
        {
            try
            {
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _concludedPrintService.RegistPrint(bindingModel.FileBarcode, bindingModel.User, bindingModel.ServiceCompanyList);
                _logger.LogInfo("RegistPrint Post");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside RegistPrint Post action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
