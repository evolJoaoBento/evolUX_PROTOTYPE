using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using System.Data;
using Newtonsoft.Json;
using Shared.ViewModels.Areas.evolDP;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using System.Data.SqlClient;
using evolUX.API.Areas.evolDP.Services.Interfaces;

namespace evolUX.API.Areas.evolDP.Controllers
{
    [Route("api/evoldp/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IClientService _projectService;
        public ClientController(IWrapperRepository repository, ILoggerService logger, IClientService projectService)
        {
            _repository = repository;
            _logger = logger;
            _projectService = projectService;
        }

        [HttpGet]
        [ActionName("GetProjects")]
        public async Task<ActionResult<ProjectListViewModel>> GetProjects([FromBody] string CompanyBusinessListJSON)
        {
            try
            {
                DataTable CompanyBusinessList = JsonConvert.DeserializeObject<DataTable>(CompanyBusinessListJSON);

                ProjectListViewModel viewmodel = await _projectService.GetProjects(CompanyBusinessList);
                _logger.LogInfo("GetProjects Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetProjects action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
