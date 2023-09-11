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

namespace evolUX.API.Areas.Reports.Controllers
{
    [Route("api/reports/RetentionReport/[action]")]
    [ApiController]
    public class RetentionReportController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IRetentionReportService _retentionReportService;
        public RetentionReportController(IWrapperRepository repository, ILoggerService logger, IRetentionReportService retentionReportService)
        {
            _repository = repository;
            _logger = logger;
            _retentionReportService = retentionReportService;
        }


        //THE SERVICECOMPANYLIST SHOULD USE A SESSION VARIABLE IN THE UI LAYER
        [HttpGet]
        [ActionName("RetentionRunReport")]
        public async Task<ActionResult<RetentionRunReportViewModel>> GetRetentionRunReport([FromBody] int BusinessAreaID, [FromQuery] int RunDate)
        {
            try
            {
                RetentionRunReportViewModel viewmodel = await _retentionReportService.GetRetentionRunReport(BusinessAreaID, RunDate);
                _logger.LogInfo("RetentionRunReport Get");
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

        //THE SERVICECOMPANYLIST SHOULD USE A SESSION VARIABLE IN THE UI LAYER
        [HttpGet]
        [ActionName("GetRetentionReport")]
        public async Task<ActionResult<RetentionReportViewModel>> GetRetentionReport([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("RunIDList", out obj);
                string RunIDListJSON = Convert.ToString(obj);
                DataTable RunIDList = JsonConvert.DeserializeObject<DataTable>(RunIDListJSON);
                dictionary.TryGetValue("BusinessAreaID", out obj);
                int BusinessAreaID = Convert.ToInt32(obj.ToString());
                
                RetentionReportViewModel viewmodel = await _retentionReportService.GetRetentionReport(RunIDList, BusinessAreaID);
                _logger.LogInfo("RetentionReport Get");
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
        [ActionName("GetRetentionInfoReport")]
        public async Task<ActionResult<RetentionInfoReportViewModel>> GetRetentionInfoReport([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("RunID", out obj);
                int RunID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("FileID", out obj);
                int FileID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("SetID", out obj);
                int SetID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("DocID", out obj);
                int DocID = Convert.ToInt32(obj.ToString());

                RetentionInfoReportViewModel viewmodel = await _retentionReportService.GetRetentionInfoReport(RunID, FileID, SetID, DocID);
                _logger.LogInfo("RetentionInfoReport Get");
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
