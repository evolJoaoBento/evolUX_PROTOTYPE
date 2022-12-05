﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using System.Data;
using Newtonsoft.Json;
using Shared.Models.General;

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
        public async Task<ActionResult<ResoursesViewModel>> GetPrinters([FromBody] string ListJSON,
                                                                        [FromQuery] bool ignoreProfiles)
        {
            try
            {
                List<string> list = JsonConvert.DeserializeObject<List<string>>(ListJSON);
                string ProfileListJSON = list[0];
                string FileSpecs = list[1];
                IEnumerable<int> profileList = JsonConvert.DeserializeObject<IEnumerable<int>>(ProfileListJSON);
                ResoursesViewModel viewmodel = await _printService.GetPrinters(profileList, FileSpecs, ignoreProfiles);
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
