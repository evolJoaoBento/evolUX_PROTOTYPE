using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
//using evolUX.API.Areas.Finishing.Models;
using SharedModels.ViewModels.Areas.Finishing;
using SharedModels.ViewModels.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using System.Data;
using Newtonsoft.Json;
using SharedModels.Models.General;

namespace evolUX.API.Areas.Finishing.Controllers
{
    [Route("api/finishing/[controller]/[action]")]
    [ApiController]
    public class PrintController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IPrintService _printService;
        public PrintController(IWrapperRepository repository, ILoggerService logger, IPrintService printService)
        {
            _repository = repository;
            _logger = logger;
            _printService = printService;
        }


        //THE SERVICECOMPANYLIST SHOULD USE A SESSION VARIABLE IN THE UI LAYER
        [HttpGet]
        [ActionName("Printers")]
        public async Task<ActionResult<ResoursesViewModel>> GetPrinters([FromBody] string ListJSON, [FromQuery] bool ignoreProfiles)
        {
            List<string> list = JsonConvert.DeserializeObject<List<string>>(ListJSON);
            string ProfileListJSON = list[0];
            string FileSpecs = list[1];
            IEnumerable<int> profileList = JsonConvert.DeserializeObject<IEnumerable<int>>(ProfileListJSON);
            try
            {
                ResoursesViewModel viewmodel = new ResoursesViewModel();
                viewmodel.Resources = await _printService.GetPrinters(profileList, FileSpecs, ignoreProfiles);
                _logger.LogInfo("Printers Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get Printers action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet]
        [ActionName("Print")]
        public async Task<ActionResult<ResultsViewModel>> Print([FromBody] string ListJSON, [FromQuery] string username, [FromQuery] int RunID, [FromQuery] int FileID, [FromQuery] string PrinterName, [FromQuery] string ServiceCompanyCode)
        {
            List<string> list = JsonConvert.DeserializeObject<List<string>>(ListJSON);
            string ProfileListJSON = list[0];
            string FileSpecs = list[1];
            IEnumerable<int> profileList = JsonConvert.DeserializeObject<IEnumerable<int>>(ProfileListJSON);
            try
            {
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = new List<Result>();
                viewmodel.Results.Add(await _printService.Print(RunID, FileID, PrinterName, ServiceCompanyCode, username));
                _logger.LogInfo("Print Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get Printers action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
