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
    public class PendingRegistriesController : ControllerBase
    {
        private readonly ILoggerService _logger;
        private readonly IPendingRegistriesService _pendingRegistriesService;
        public PendingRegistriesController(IWrapperRepository repository, ILoggerService logger, IPendingRegistriesService pendingRegistriesService)
        {
            _logger = logger;
            _pendingRegistriesService = pendingRegistriesService;
        }



        [HttpGet]
        [ActionName("PendingRegistries")]
        public async Task<ActionResult<PendingRegistriesViewModel>> GetPendingRegistries([FromBody] string ServiceCompanyListJSON)
        {
            DataTable ServiceCompanyList = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyListJSON);
            try
            {
                PendingRegistriesViewModel viewmodel = await _pendingRegistriesService.GetPendingRegistries(ServiceCompanyList);
                _logger.LogInfo("PendingRegistries Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get PendingRegistries action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
