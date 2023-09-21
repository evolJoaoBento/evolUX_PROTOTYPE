using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
//using Shared.Models.Areas.Finishing;
using Shared.ViewModels.Areas.Reports;
using Shared.Models.Areas.Finishing;
using evolUX.API.Areas.Reports.Services.Interfaces;
//using Shared.Models.Areas.Reports;
using System.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using System.Data.SqlClient;
using evolUX.API.Areas.Reports.Services;

namespace evolUX.API.Areas.Reports.Controllers
{
    [Route("api/reports/DependentProduction/[action]")]
    [ApiController]
    public class DependentProductionController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IDependentProductionService _dependentProductionService;
        public DependentProductionController(IWrapperRepository repository, ILoggerService logger, IDependentProductionService dependentProductionService)
        {
            _repository = repository;
            _logger = logger;
            _dependentProductionService = dependentProductionService;
        }
    

        [HttpGet]
        [ActionName("GetDependentProduction")]
        public async Task<ActionResult<DependentProductionViewModel>> GetDependentProduction([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ServiceCompanyList", out obj);
                string ServiceCompanyListJSON = Convert.ToString(obj);
                DataTable ServiceCompanyList = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyListJSON);

                DependentProductionViewModel viewmodel = await _dependentProductionService.GetDependentProduction(ServiceCompanyList);
                _logger.LogInfo("DependentProduction Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
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
