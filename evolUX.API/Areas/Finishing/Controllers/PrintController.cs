using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using Shared.ViewModels.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using System.Data;
using Newtonsoft.Json;
using Shared.Models.General;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using System.Data.SqlClient;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.Finishing;

namespace evolUX.API.Areas.Finishing.Controllers
{
    [Route("api/finishing/[controller]/[action]")]
    [ApiController]
    public class PrintController : ControllerBase
    {
        private readonly ILoggerService _logger;
        private readonly IPrintService _printService;
        public PrintController(IWrapperRepository repository, ILoggerService logger, IPrintService printService)
        {
            _logger = logger;
            _printService = printService;
        }


        //THE SERVICECOMPANYLIST SHOULD USE A SESSION VARIABLE IN THE UI LAYER
        [HttpGet]
        [ActionName("Printers")]
        public async Task<ActionResult<PrinterViewModel>> GetPrinters([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ProfileList", out obj);
                string ProfileListJSON = Convert.ToString(obj);
                dictionary.TryGetValue("FilesSpecs", out obj);
                string FileSpecs = Convert.ToString(obj);
                dictionary.TryGetValue("IgnoreProfiles", out obj);
                bool IgnoreProfiles = Convert.ToBoolean(obj);

                IEnumerable<int> profileList = JsonConvert.DeserializeObject<IEnumerable<int>>(ProfileListJSON);
                PrinterViewModel viewmodel = await _printService.GetPrinters(profileList, FileSpecs, IgnoreProfiles);
                _logger.LogInfo("Printers Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
        public async Task<ActionResult<ResultsViewModel>> Print([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("Username", out obj);
                string Username = Convert.ToString(obj);
                dictionary.TryGetValue("UserID", out obj);
                int UserID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("FilePath", out obj);
                string FilePath = Convert.ToString(obj);
                dictionary.TryGetValue("FileID", out obj);
                int FileID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("RunID", out obj);
                int RunID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("Printer", out obj);
                string Printer = Convert.ToString(obj);
                dictionary.TryGetValue("ServiceCompanyCode", out obj);
                string ServiceCompanyCode = Convert.ToString(obj);
                dictionary.TryGetValue("FileName", out obj);
                string FileName = Convert.ToString(obj);
                dictionary.TryGetValue("ShortFileName", out obj);
                string ShortFileName = Convert.ToString(obj); 
                Result viewmodel = await _printService.Print(RunID, FileID, Printer, ServiceCompanyCode, 
                    Username, UserID, FilePath, FileName, ShortFileName);
                _logger.LogInfo("Print Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
