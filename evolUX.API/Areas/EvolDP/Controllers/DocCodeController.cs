using evolUX.API.Data.Interfaces;
using evolUX.API.Services.Interfaces;
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
        public DocCodeController(IWrapperRepository repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ActionName("getDocCode")]
        public async Task<ActionResult<List<dynamic>>> GetDocCode()
        {
            try
            {
                var docCodeList = await _repository.DocCode.GetDocCode();
                _logger.LogInfo("DocCode Get");
                return Ok(docCodeList);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetDocCode action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("getDocCodeLevel1")]
        public async Task<ActionResult<List<dynamic>>> GetDocCodeLevel1([FromBody] dynamic data)
        {
            try
            {
                var converter = new ExpandoObjectConverter();
                var exObjExpandoObject = JsonConvert.DeserializeObject<ExpandoObject>(data.ToString(), converter) as dynamic;
                var docCodeList = await _repository.DocCode.GetDocCodeLevel1(exObjExpandoObject);
                _logger.LogInfo("DocCodeLevel1 Get");
                return Ok(docCodeList);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetDocCode action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("getDocCodeLevel2")]
        public async Task<ActionResult<List<dynamic>>> GetDocCodeLevel2([FromBody] dynamic data)
        {
            try
            {
                var converter = new ExpandoObjectConverter();
                var exObjExpandoObject = JsonConvert.DeserializeObject<ExpandoObject>(data.ToString(), converter) as dynamic;
                var docCodeList = await _repository.DocCode.GetDocCodeLevel2(exObjExpandoObject);
                _logger.LogInfo("DocCodeLevel2 Get");
                return Ok(docCodeList);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetDocCode action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("getDocCodeConfig")]
        public async Task<ActionResult<dynamic>> GetDocCodeConfig([FromBody] dynamic data)
        {
            try
            {
                var converter = new ExpandoObjectConverter();
                var exObjExpandoObject = JsonConvert.DeserializeObject<ExpandoObject>(data.ToString(), converter) as dynamic;
                var docCodeConfig = await _repository.DocCode.GetDocCodeConfig(exObjExpandoObject);
                _logger.LogInfo("DocCodeConfig Get");
                return Ok(docCodeConfig);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetDocCode action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("addExceptionDocCode")]
        public async Task<ActionResult<dynamic>> AddExceptionDocCode([FromBody] dynamic data)
        {
            try
            {
                var converter = new ExpandoObjectConverter();
                var exObjExpandoObject = JsonConvert.DeserializeObject<ExpandoObject>(data.ToString(), converter) as dynamic;
                //dynamic 
                //var docCodeConfig = await _repository.DocCode.GetDocCodeConfig(exObjExpandoObject);
                //_logger.LogInfo("DocCodeConfig Get");
                return Ok();
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetDocCode action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }



    }
}
