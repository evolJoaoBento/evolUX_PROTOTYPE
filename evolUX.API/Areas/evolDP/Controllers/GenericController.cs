using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using System.Data;
using Newtonsoft.Json;
using Shared.ViewModels.Areas.evolDP;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using System.Data.SqlClient;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using evolUX.API.Areas.evolDP.Services;
using Shared.Models.Areas.evolDP;

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
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]//TODO: need to ask about authorization here
        [ActionName("GetCompanies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies([FromBody] Dictionary<string, object> dictionary)
        {
            try
            {
                IEnumerable<Company> companies = new List<Company>();
                object obj;
                dictionary.TryGetValue("CompanyID", out obj);
                int value = 0;
                int? companyID = null;
                if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                    companyID = value;
                DataTable companyList = new DataTable();
                if (companyID == null)
                {
                    dictionary.TryGetValue("CompanyList", out obj);
                    string companyListJSON = Convert.ToString(obj);
                    companyList = JsonConvert.DeserializeObject<DataTable>(companyListJSON).DefaultView.ToTable(false, "ID");
                    companies = await _genericService.GetCompanies(companyList);
                }
                else
                {
                    companies = await _genericService.GetCompanies((int)companyID);
                }

                _logger.LogInfo("Companies Get");
                return Ok(companies);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside GetCompanies action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("SetCompany")]
        public async Task<ActionResult<Company>> SetCompany([FromBody] string CompanyJSON)
        {
            Company company = JsonConvert.DeserializeObject<Company>(CompanyJSON);
            try
            {

                Company newCompany = await _genericService.SetCompany(company);
                _logger.LogInfo("SetCompany Get");
                return Ok(newCompany);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get SetCompany action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [ActionName("GetCompanyBusiness")]
        public async Task<ActionResult<BusinessViewModel>> GetCompanyBusiness([FromBody] Dictionary<string, object> dictionary)
        {
            object obj;
            dictionary.TryGetValue("CompanyID", out obj);
            int value = 0;
            int companyID = 0;
            if (obj != null && Int32.TryParse(Convert.ToString(obj), out value))
                companyID = value;

            DataTable CompanyList = null;
            dictionary.TryGetValue("CompanyList", out obj);
            if (obj != null)
            {
                string companyListJSON = Convert.ToString(obj);
                if (!string.IsNullOrEmpty(companyListJSON))
                {
                    CompanyList = JsonConvert.DeserializeObject<DataTable>(companyListJSON).DefaultView.ToTable(false, "ID");
                }
            }
            try
            {
                BusinessViewModel viewmodel = new BusinessViewModel();
                viewmodel.CompanyBusiness = await _genericService.GetCompanyBusiness(companyID, CompanyList);
                if (companyID > 0)
                {
                    viewmodel.Company = (await _genericService.GetCompanies(companyID)).First();

                }
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
        [ActionName("SetBusiness")]
        public async Task<ActionResult> SetBusiness([FromBody] string BusinessJSON)
        {
            Business business = JsonConvert.DeserializeObject<Business>(BusinessJSON);
            try
            {

                await _genericService.SetBusiness(business);
                _logger.LogInfo("SetBusiness Get");
                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Internal Server Error");
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get SetBusiness action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
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
