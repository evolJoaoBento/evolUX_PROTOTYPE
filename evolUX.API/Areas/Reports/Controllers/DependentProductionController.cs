using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using Shared.ViewModels.Areas.Reports;
using evolUX.API.Areas.Reports.Services.Interfaces;
using System.Data;
using Newtonsoft.Json;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using System.Data.SqlClient;

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
        [ActionName("GetDependentPrintsProduction")]
        public async Task<ActionResult<DependentProductionViewModel>> GetDependentPrintsProduction([FromBody] int i)
        {
            try
            {
                object obj;
                //dictionary.TryGetValue("ServiceCompanyList", out obj);
                //string ServiceCompanyListJSON = Convert.ToString(obj);
                DataTable ServiceCompanyList= null; //= JsonConvert.DeserializeObject<DataTable>(ServiceCompanyListJSON);

                DependentProductionViewModel viewmodel = await _dependentProductionService.GetDependentPrintsProduction(ServiceCompanyList);
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
