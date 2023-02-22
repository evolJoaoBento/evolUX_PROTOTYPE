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
using Shared.ViewModels.General;
using System.Reflection.Emit;
using Dapper;
using System.Data;

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

        [HttpGet]
        [ActionName("DocCodeConfig")]
        public async Task<ActionResult<DocCodeConfigViewModel>> GetDocCodeConfig([FromBody] Dictionary<string, object> dictionary)
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

                DocCodeConfigViewModel viewmodel = await _docCodeService.GetDocCodeConfig(docCodeID, startDate, maxDateFlag);
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

        [HttpGet]
        [ActionName("ExceptionLevel")]
        public async Task<ActionResult<ExceptionLevelViewModel>> GetExceptionLevel([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("Level", out obj);
                int level = Convert.ToInt32(obj.ToString());

                ExceptionLevelViewModel viewmodel = await _docCodeService.GetExceptionLevel(level);
                _logger.LogInfo("GetDocExceptionsLevel Get");
                return Ok(viewmodel);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get GetDocExceptionsLevel action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpGet]
        [ActionName("RegistDocCodeConfig")]
        public async Task<ActionResult<DocCodeConfigViewModel>> RegistDocCodeConfig([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("DocCode", out obj);
                DocCode? docCode = null;
                if (obj != null)
                    docCode = JsonConvert.DeserializeObject<DocCode>(Convert.ToString(obj));

                DocCodeConfigViewModel viewmodel = new DocCodeConfigViewModel();
                viewmodel.DocCode = (await _docCodeService.SetDocCodeConfig(docCode)).DocCodeList.First();

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

        [HttpGet]
        [ActionName("ChangeDocCode")]
        public async Task<ActionResult<DocCodeConfigViewModel>> ChangeDocCode([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("DocCode", out obj);
                DocCode? docCode = null;
                if (obj != null)
                    docCode = JsonConvert.DeserializeObject<DocCode>(Convert.ToString(obj));

                DocCodeConfigViewModel viewmodel = new DocCodeConfigViewModel();
                viewmodel.DocCode = (await _docCodeService.ChangeDocCode(docCode)).DocCodeList.First();

                _logger.LogInfo("ChangeDocCode Get");
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

        [HttpGet]
        [ActionName("DeleteDocCodeConfig")]
        public async Task<ActionResult<ResultsViewModel>> DeleteDocCodeConfig([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("DocCodeID", out obj);
                int docCodeID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("StartDate", out obj);
                int startDate = Convert.ToInt32(obj.ToString());

                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _docCodeService.DeleteDocCodeConfig(docCodeID, startDate);
                _logger.LogInfo("DeleteDocCodeConfig");
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

        [HttpGet]
        [ActionName("DeleteDocCode")]
        public async Task<ActionResult<ResultsViewModel>> DeleteDocCode([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("DocCode", out obj);
                DocCode? docCode = null;
                if (obj != null)
                    docCode = JsonConvert.DeserializeObject<DocCode>(Convert.ToString(obj));

                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _docCodeService.DeleteDocCode(docCode.DocCodeID);
                _logger.LogInfo("DeleteDocCode");
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

        [HttpGet]
        [ActionName("GetCompatibility")]
        public async Task<ActionResult<DocCodeCompatibilityViewModel>> GetCompatibility([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("DocCodeID", out obj);
                int docCodeID = Convert.ToInt32(obj.ToString());

                DocCodeCompatibilityViewModel viewmodel = new DocCodeCompatibilityViewModel();
                viewmodel.AggDocCodeList = await _docCodeService.GetCompatibility(docCodeID);
                _logger.LogInfo("GetCompatibility");
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

        [HttpGet]
        [ActionName("ChangeCompatibility")]
        public async Task<ActionResult<DocCodeCompatibilityViewModel>> ChangeCompatibility([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("DocCodeID", out obj);
                int docCodeID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("DocCodeList", out obj);
                string DocCodeListJSON = Convert.ToString(obj);

                List<string> dcList = JsonConvert.DeserializeObject<List<string>>(DocCodeListJSON);
                DataTable docCodeList = new DataTable();
                docCodeList.Columns.Add("ID", typeof(int));

                foreach (string value in dcList)
                {
                    DataRow row = docCodeList.NewRow();
                    row["ID"] = Int32.Parse(value);
                    docCodeList.Rows.Add(row);
                }
                DocCodeCompatibilityViewModel viewmodel = new DocCodeCompatibilityViewModel();
                viewmodel.AggDocCodeList = await _docCodeService.ChangeCompatibility(docCodeID, docCodeList);
                _logger.LogInfo("ChangeCompatibility");
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

    }
}
