using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
//using evolUX.API.Areas.Finishing.Models;
using evolUX.API.Areas.Finishing.ViewModels;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using evolUX.API.Areas.Finishing.Models;
using System.Data;

namespace evolUX.API.Areas.Finishing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionReportController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerManager _logger;
        private readonly IProductionReportService _productionReportService;
        public ProductionReportController(IWrapperRepository repository, ILoggerManager logger, IProductionReportService productionReportService)
        {
            _repository = repository;
            _logger = logger;
            _productionReportService = productionReportService;
        }


        //THE SERVICECOMPANYLIST SHOULD USE A SESSION VARIABLE IN THE UI LAYER
        [HttpGet]
        [ActionName("ProductionRunReport")]
        public async Task<ActionResult<ProductionRunReportViewModel>> GetProductionRunReport([FromBody] DataTable ServiceCompanyList)
        {
            try
            {
                ProductionRunReportViewModel viewmodel = new ProductionRunReportViewModel();
                viewmodel.ProductionRunReport = await _productionReportService.GetProductionRunReport(ServiceCompanyList);
                _logger.LogInfo("ProductionRunReport Get");
                return Ok(viewmodel);
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
        [ActionName("ProductionReport")]
        public async Task<ActionResult<ProductionReportViewModel>> GetProductionReport([FromQuery] int RunID, [FromQuery] int ServiceCompanyID)
        {
            try
            {
                ProductionReportViewModel viewmodel = new ProductionReportViewModel();
                viewmodel.ProductionReport = await _productionReportService.GetProductionReport(RunID, ServiceCompanyID);
                foreach(ProductionDetailInfo pdi in viewmodel.ProductionReport)
                {
                    pdi.ProductionDetailReport = await _productionReportService.GetProductionDetailReport(pdi.RunID,pdi.ServiceCompanyID,pdi.PaperMediaID,pdi.StationMediaID,pdi.ExpeditionType,pdi.ExpCode,pdi.HasColorPages);
                }
                _logger.LogInfo("ProductionReport Get");
                return Ok(viewmodel);
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
