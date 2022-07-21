using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.EvolDP.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace evolUX.Areas.EvolDP.Controllers
{
    [Route("evoldp/doccode/[action]")]

    [ApiController]
    public class DocCodeController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerManager _logger;
        private readonly IDocCodeService _docCodeService;
        public DocCodeController(IWrapperRepository repository, ILoggerManager logger, IDocCodeService docCodeService)
        {
            _repository = repository;
            _logger = logger;
            _docCodeService = docCodeService;
        }

        [HttpGet]
        [ActionName("DocCode")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<dynamic>>> GetDocCode()
        {
            try
            {
                var docCodeList = await _docCodeService.GetDocCode();
                _logger.LogInfo("DocCode Get");
                return Ok(docCodeList);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DocCode action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("DocCodeLevel1")]
        public async Task<ActionResult<List<dynamic>>> GetDocCodeLevel1([FromBody] dynamic data)
        {
            try
            {
                var converter = new ExpandoObjectConverter();
                var exObjExpandoObject = JsonConvert.DeserializeObject<ExpandoObject>(data.ToString(), converter) as dynamic;
                var docCodeList = await _docCodeService.GetDocCodeLevel1(exObjExpandoObject);
                _logger.LogInfo("DocCodeLevel1 Get");
                return Ok(docCodeList);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DocCodeLevel1 action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("DocCodeLevel2")]
        public async Task<ActionResult<List<dynamic>>> GetDocCodeLevel2([FromBody] dynamic data)
        {
            try
            {
                var converter = new ExpandoObjectConverter();
                var exObjExpandoObject = JsonConvert.DeserializeObject<ExpandoObject>(data.ToString(), converter) as dynamic;
                //var docCodeList = await _repository.DocCode.GetDocCodeLevel2(exObjExpandoObject);
                var docCodeList = await _docCodeService.GetDocCodeLevel2(exObjExpandoObject);
                _logger.LogInfo("DocCodeLevel2 Get");
                return Ok(docCodeList);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DocCodeLevel2 action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("DocCodeConfig")]
        public async Task<ActionResult<dynamic>> GetDocCodeConfig([FromBody] dynamic data)
        {
            try
            {
                var converter = new ExpandoObjectConverter();
                var exObjExpandoObject = JsonConvert.DeserializeObject<ExpandoObject>(data.ToString(), converter) as dynamic;
                //var docCodeConfig = await _repository.DocCode.GetDocCodeConfig(exObjExpandoObject);
                var docCodeConfig = await _docCodeService.GetDocCodeConfig(exObjExpandoObject);
                _logger.LogInfo("DocCodeConfig Get");
                return Ok(docCodeConfig);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DocCodeConfig action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("DocCodeExceptionOptions")]
        public async Task<ActionResult<dynamic>> AddExceptionDocCode([FromBody] dynamic data)
        {
            try
            {
                var converter = new ExpandoObjectConverter();
                var exObjExpandoObject = JsonConvert.DeserializeObject<ExpandoObject>(data.ToString(), converter) as dynamic;
                //dynamic 
                var docCodeConfig = await _docCodeService.GetDocCodeExceptionOptions(exObjExpandoObject);
                _logger.LogInfo("DocCodeException Get");
                return Ok();
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get ExceptoionDocCodeOptions action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }



    }
}
