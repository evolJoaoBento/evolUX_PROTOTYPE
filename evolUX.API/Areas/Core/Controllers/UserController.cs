using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
//using Shared.Models.Areas.Finishing;
using Shared.ViewModels.Areas.Finishing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using System.Data;
using Newtonsoft.Json;
using Shared.Models.Areas.Core;
using Shared.ViewModels.General;
using evolUX.API.Areas.Finishing.Services;
using evolUX.API.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace evolUX.API.Areas.Core.Controllers
{
    [Route("api/core/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IUserService _userService;
        public UserController(IWrapperRepository repository, ILoggerService logger, IUserService userService)
        {
            _repository = repository;
            _logger = logger;
            _userService = userService;
        }


        //MISSING SYSTEM VARIABLES & FLOW VARIABLES & ACTION PARAMETERS(XML READ)
        [HttpGet]
        [ActionName("ChangeCulture")]
        public async Task<ActionResult<ResultsViewModel>> ChangeCulture([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("UserID", out obj);
                int userID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("Culture", out obj);
                string culture = Convert.ToString(obj);
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _userService.ChangeCulture(userID, culture);
                _logger.LogInfo("ChangeCulture Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside SessionVariables Get action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
