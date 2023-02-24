using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using System.Data;
using Newtonsoft.Json;
using evolUX.API.Areas.Finishing.Services;
using Shared.Models.General;
using Shared.ViewModels.General;
using Shared.ViewModels.Areas.evolDP;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using System.Data.SqlClient;
using Shared.Models.Areas.Finishing;
using System.Collections.Generic;

namespace evolUX.API.Areas.Finishing.Controllers
{
    [Route("api/finishing/[controller]/[action]")]
    [ApiController]
    public class ExpeditionReportController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IExpeditionReportService _expeditionService;
        public ExpeditionReportController(IWrapperRepository repository, ILoggerService logger, IExpeditionReportService expeditionService)
        {
            _repository = repository;
            _logger = logger;
            _expeditionService = expeditionService;
        }


        //THE SERVICECOMPANYLIST SHOULD USE A SESSION VARIABLE IN THE UI LAYER
        [HttpGet]
        [ActionName("GetCompanyBusiness")]
        public async Task<ActionResult<BusinessViewModel>> GetCompanyBusiness([FromBody] string CompanyBusinessListJSON)
        {
            DataTable CompanyBusinessList = JsonConvert.DeserializeObject<DataTable>(CompanyBusinessListJSON);
            try
            {
                BusinessViewModel viewmodel = new BusinessViewModel();
                viewmodel.CompanyBusiness = await _expeditionService.GetCompanyBusiness(CompanyBusinessList);
                _logger.LogInfo("GetCompanyBusiness Get");
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
        [ActionName("GetPendingExpeditionFiles")]
        public async Task<ActionResult<ExpeditionListViewModel>> GetPendingExpeditionFiles([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("BusinessID", out obj);
                int BusinessID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("ServiceCompanyList", out obj);
                string ServiceCompanyListJSON = Convert.ToString(obj);

                DataTable ServiceCompanies = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyListJSON);
                DataTable ServiceCompanyList = ServiceCompanies.DefaultView.ToTable(false, "ID");

                ExpeditionListViewModel viewmodel = await _expeditionService.GetPendingExpeditionFiles(BusinessID, ServiceCompanyList);
                _logger.LogInfo("GetPendingExpeditionFiles Get");
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
        [ActionName("RegistExpeditionReport")]
        public async Task<ActionResult<ResultsViewModel>> RegistExpeditionReport([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("UserID", out obj);
                int userID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("UserName", out obj);
                string userName = Convert.ToString(obj);
                dictionary.TryGetValue("ExpFiles", out obj);
                string expFilesJSON = Convert.ToString(obj);
                List<RegistExpReportElement> expFiles = JsonConvert.DeserializeObject<List<RegistExpReportElement>>(expFilesJSON);

                Result viewmodel = await _expeditionService.RegistExpeditionReport(expFiles, userName, userID);
                _logger.LogInfo("RegistExpeditionReport Get");
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
        [ActionName("GetExpeditionReportList")]
        public async Task<ActionResult<ExpeditionListViewModel>> GetExpeditionReportList([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                object obj;
                dictionary.TryGetValue("BusinessID", out obj);
                int BusinessID = Convert.ToInt32(obj.ToString());
                dictionary.TryGetValue("ServiceCompanyList", out obj);
                string ServiceCompanyListJSON = Convert.ToString(obj);

                DataTable ServiceCompanies = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyListJSON);
                DataTable ServiceCompanyList = ServiceCompanies.DefaultView.ToTable(false, "ID");

                ExpeditionListViewModel viewmodel = await _expeditionService.GetExpeditionReportList(BusinessID, ServiceCompanyList);
                _logger.LogInfo("GetPendingExpeditionFiles Get");
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
