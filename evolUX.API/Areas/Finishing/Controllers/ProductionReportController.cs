using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
//using Shared.Models.Areas.Finishing;
using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.Models.Areas.Finishing;
using System.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using System.Data.SqlClient;

namespace evolUX.API.Areas.Finishing.Controllers
{
    [Route("api/finishing/[controller]/[action]")]
    [ApiController]
    public class ProductionReportController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IProductionReportService _productionReportService;
        public ProductionReportController(IWrapperRepository repository, ILoggerService logger, IProductionReportService productionReportService)
        {
            _repository = repository;
            _logger = logger;
            _productionReportService = productionReportService;
        }


        //THE SERVICECOMPANYLIST SHOULD USE A SESSION VARIABLE IN THE UI LAYER
        [HttpGet]
        [ActionName("ProductionRunReport")]
        public async Task<ActionResult<ProductionRunReportViewModel>> GetProductionRunReport([FromBody] int ServiceCompanyID)
        {
            try
            {
                ProductionRunReportViewModel viewmodel = new ProductionRunReportViewModel();
                viewmodel.ProductionRunReport = await _productionReportService.GetProductionRunReport(ServiceCompanyID);
                _logger.LogInfo("ProductionRunReport Get");
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
        [ActionName("GetProductionReport")]
        public async Task<ActionResult<ProductionReportViewModel>> GetProductionPrinterReport([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ProfileList", out obj);
                string ProfileListJSON = Convert.ToString(obj);
                dictionary.TryGetValue("RunIDList", out obj);
                string RunIDListJSON = Convert.ToString(obj);
                DataTable RunIDList = JsonConvert.DeserializeObject<DataTable>(RunIDListJSON);

                dictionary.TryGetValue("ServiceCompanyID", out obj);
                int ServiceCompanyID = Convert.ToInt32(obj.ToString());
                IEnumerable<int> ProfileList = JsonConvert.DeserializeObject<IEnumerable<int>>(ProfileListJSON);
                dictionary.TryGetValue("FilterOnlyPrint", out obj);
                bool FilterOnlyPrint = Convert.ToBoolean(obj.ToString());

                ProductionReportViewModel viewmodel = await _productionReportService.GetProductionReport(ProfileList, RunIDList, ServiceCompanyID, FilterOnlyPrint);
                _logger.LogInfo("ProductionReport Get");
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
        [ActionName("GetProductionReportFilters")]
        public async Task<ActionResult<IEnumerable<ProductionDetailInfo>>> GetProductionPrinterReportFilters([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("ProfileList", out obj);
                string ProfileListJSON = Convert.ToString(obj);
                dictionary.TryGetValue("RunIDList", out obj);
                string RunIDListJSON = Convert.ToString(obj);
                DataTable RunIDList = JsonConvert.DeserializeObject<DataTable>(RunIDListJSON);

                dictionary.TryGetValue("ServiceCompanyID", out obj);
                int ServiceCompanyID = Convert.ToInt32(obj.ToString());
                IEnumerable<int> ProfileList = JsonConvert.DeserializeObject<IEnumerable<int>>(ProfileListJSON);
                dictionary.TryGetValue("FilterOnlyPrint", out obj);
                bool FilterOnlyPrint = Convert.ToBoolean(obj.ToString());

                IEnumerable<ProductionDetailInfo> viewmodel = await _productionReportService.GetProductionReportFilters(ProfileList, RunIDList, ServiceCompanyID, FilterOnlyPrint);
                _logger.LogInfo("ProductionReportFilters Get");
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
