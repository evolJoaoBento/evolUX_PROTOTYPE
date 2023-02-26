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
    public class GenericController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IGenericService _genericService;
        public GenericController(IWrapperRepository repository, ILoggerService logger, IGenericService genericService)
        {
            _repository = repository;
            _logger = logger;
            _genericService = genericService;
        }

        [HttpGet]
        [ActionName("GetProjects")]
        public async Task<ActionResult<ProjectListViewModel>> GetProjects([FromBody] string CompanyBusinessListJSON)
        {
            try
            {
                DataTable CompanyBusinessList = JsonConvert.DeserializeObject<DataTable>(CompanyBusinessListJSON);
                ProjectListViewModel viewmodel = await _genericService.GetProjects(CompanyBusinessList);
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
        [HttpGet]
        [ActionName("GetParameters")]
        public async Task<ActionResult<ConstantParameterViewModel>> GetParameters()
        {
            try
            {
                ConstantParameterViewModel viewmodel = await _genericService.GetParameters();
                _logger.LogInfo("GetParameters Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get GetParameters action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpGet]
        [ActionName("SetParameter")]
        public async Task<ActionResult<ConstantParameterViewModel>> SetParameter([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ParameterID", out obj);
                int parameterID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("ParameterRef", out obj);
                string parameterRef = Convert.ToString(obj);
                dictionary.TryGetValue("ParameterValue", out obj);
                int parameterValue = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("ParameterDescription", out obj);
                string parameterDescription = Convert.ToString(obj);

                ConstantParameterViewModel viewmodel = await _genericService.SetParameter(parameterID, parameterRef, parameterValue, parameterDescription);
                _logger.LogInfo("SetParameter Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get SetParameter action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpGet]
        [ActionName("DeleteParameter")]
        public async Task<ActionResult<ConstantParameterViewModel>> DeleteParameter([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ParameterID", out obj);
                int parameterID = Convert.ToInt32(obj.ToString());

                ConstantParameterViewModel viewmodel = await _genericService.DeleteParameter(parameterID);
                _logger.LogInfo("DeleteParameter Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DeleteParameter action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }
    }
}
