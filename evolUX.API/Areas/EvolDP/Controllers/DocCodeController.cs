using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.EvolDP.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using System.Data.SqlClient;
using Shared.ViewModels.Areas.evolDP;
using Shared.Models.Areas.evolDP;
using System.Globalization;

namespace evolUX.Areas.EvolDP.Controllers
{
    [Route("api/evoldp/doccode/[action]")]

    [ApiController]
    public class DocCodeController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IDocCodeService _docCodeService;
        public DocCodeController(IWrapperRepository repository, ILoggerService logger, IDocCodeService docCodeService)
        {
            _repository = repository;
            _logger = logger;
            _docCodeService = docCodeService;
        }
        //TODO: DOCUMENT UNTESTED
        //TODO: HANDLE HTTP RESPONSES
        [HttpGet]
        [ActionName("DocCodeGroup")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<DocCodeViewModel>> GetDocCodeGroup()
        {
            try
            {
                DocCodeViewModel viewmodel = await _docCodeService.GetDocCodeGroup();
                _logger.LogInfo("DocCodeGroup Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DocCodeGroup action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("DocCode")]
        public async Task<ActionResult<DocCodeViewModel>> GetDocCode([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("DocLayout", out obj);
                string docLayout = Convert.ToString(obj);
                dictionary.TryGetValue("DocType", out obj);
                string docType = Convert.ToString(obj);

                DocCodeViewModel viewmodel = await _docCodeService.GetDocCode(docLayout, docType);
                _logger.LogInfo("DocCode Get");
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

        //TODO: DOCUMENT UNTESTED
        //TODO: HANDLE HTTP RESPONSES
        [HttpGet]
        [ActionName("DocCodeConfig")]
        public async Task<ActionResult<DocCodeViewModel>> GetDocCodeConfig([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("DocCodeID", out obj);
                int docCodeID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("StartDate", out obj);
                DateTime? startDate = (DateTime?)obj;
                dictionary.TryGetValue("MaxDateFlag", out obj);
                bool? maxDateFlag = (bool?)obj;

                DocCodeViewModel viewmodel = await _docCodeService.GetDocCodeConfig(docCodeID, startDate, maxDateFlag);
                _logger.LogInfo("DocCodeConfig Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DocCodeLevel2 action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet]
        [ActionName("DocCodeConfigOptions")]
        public async Task<ActionResult<DocCodeConfigOptionsViewModel>> GetDocCodeConfigOptions([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("DocCode", out obj);
                DocCode? docCode = null;
                if (obj != null)
                    docCode = JsonConvert.DeserializeObject<DocCode>(Convert.ToString(obj));

                DocCodeConfigOptionsViewModel viewmodel = await _docCodeService.GetDocCodeConfigOptions(docCode);
                _logger.LogInfo("DocCodeConfigOptions Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DocCodeConfigOptions action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPost]
        [ActionName("RegistDocCodeConfig")]
        public async Task<ActionResult<DocCodeViewModel>> RegistDocCodeConfig([FromBody] DocCode docCode)
        {
            try
            {
                await _docCodeService.PostDocCodeConfig(docCode);
                DocCodeViewModel viewmodel = await _docCodeService.GetDocCodeConfig(docCode.DocCodeID, DateTime.ParseExact(docCode.DocCodeConfigs[0].StartDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture), null);
                _logger.LogInfo("RegistDocCodeConfig Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get RegistDocCodeConfig action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpDelete("{docCodeID}")]
        [ActionName("DeleteDocCode")]
        public async Task<ActionResult<DocCodeResultsViewModel>> DeleteDocCode([FromRoute] int docCodeID)
        {
            try
            {
                DocCodeResultsViewModel viewmodel = new DocCodeResultsViewModel();
                viewmodel.Results = await _docCodeService.DeleteDocCode(docCodeID);
                _logger.LogInfo("DocCodeException Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get ExceptoionDocCodeOptions action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }
        //Podia se mandar um aviso sobre algo nao ser compativel no futuro
        [HttpGet("{docCodeID}")]
        [ActionName("Compatibility")]
        public async Task<ActionResult<DocCodeCompatabilityViewModel>> CompatibilityOptions([FromRoute] int docCodeID)
        {
            try
            {
                DocCodeCompatabilityViewModel viewmodel = new DocCodeCompatabilityViewModel();
                viewmodel.DocCode = await _docCodeService.GetAggregateDocCode(docCodeID);
                viewmodel.DocCodeList = await _docCodeService.GetAggregateDocCodes(docCodeID);
                _logger.LogInfo("CompatibilityOptions");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside CompatibilityOptions action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPut()]
        [ActionName("ChangeCompatibility")]
        public async Task<ActionResult<DocCodeResultsViewModel>> ChangeCompatibility([FromBody] DocCodeCompatabilityViewModel model)
        {
            try
            {
                await _docCodeService.ChangeCompatibility(model);
                _logger.LogInfo("ChangeCompatibility");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside ChangeCompatibility action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }
    }
}
