using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.EvolDP.Models;
using evolUX.API.Areas.EvolDP.ViewModels;
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
        [ActionName("DocCode")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<DocCodeViewModel>> GetDocCodeGroup()
        {
            try
            {
                DocCodeViewModel viewmodel = await _docCodeService.GetDocCodeGroup();
                _logger.LogInfo("DocCodeGroup Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DocCodeGroup action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        //TODO: DOCUMENT UNTESTED
        //TODO: HANDLE HTTP RESPONSES
        [HttpGet("details")]
        [ActionName("DocCode")]
        public async Task<ActionResult<DocCodeViewModel>> GetDocCode([FromQuery] string docLayout, [FromQuery] string docType)
        {
            try
            {
                DocCodeViewModel viewmodel = await _docCodeService.GetDocCode(docLayout, docType);
                _logger.LogInfo("DocCode Get");
                return Ok(viewmodel);
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
        [HttpGet("{ID}")]
        [ActionName("DocCodeConfig")]
        public async Task<ActionResult<DocCodeConfigViewModel>> GetDocCodeConfig([FromRoute] string ID)
        {
            try
            {
                DocCodeConfigViewModel viewmodel = await _docCodeService.GetDocCodeConfig(ID);
                _logger.LogInfo("DocCodeConfig Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DocCodeLevel2 action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        //TODO: DOCUMENT UNTESTED
        //TODO: HANDLE HTTP RESPONSES
        [HttpGet("{ID}/details")]
        [ActionName("DocCodeConfig")]
        public async Task<ActionResult<DocCodeConfigViewModel>> GetDocCodeConfig([FromRoute] string ID, [FromQuery] int startdate)
        {
            try
            {
                DocCodeConfigViewModel viewmodel = new DocCodeConfigViewModel();
                List<DocCodeConfig> list = new List<DocCodeConfig>();
                list.Add(await _docCodeService.GetDocCodeConfig(ID, startdate));
                viewmodel.DocCodeConfigList = list;
                _logger.LogInfo("DocCodeConfig Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get DocCodeConfig action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        //TODO: DOCUMENT UNTESTED
        //TODO: HANDLE HTTP RESPONSES
        [HttpGet("{ID}")]
        [ActionName("DocCodeConfigOptions")]
        public async Task<ActionResult<DocCodeConfigOptionsViewModel>> AddDocCodeConfig([FromRoute] string ID)
        {
            try
            {
                DocCodeConfigOptionsViewModel viewmodel = new DocCodeConfigOptionsViewModel();
                viewmodel.DocCodeConfig = await _docCodeService.GetDocCodeConfigOptions(ID);
                viewmodel.DocExceptionslevel1 = await _docCodeService.GetDocExceptionsLevel1();
                viewmodel.DocExceptionslevel2 = await _docCodeService.GetDocExceptionsLevel2();
                viewmodel.DocExceptionslevel3 = await _docCodeService.GetDocExceptionsLevel3();
                viewmodel.EnvelopeMediaGroups = await _docCodeService.GetEnvelopeMediaGroups(viewmodel.DocCodeConfig.EnvMedia);
                viewmodel.AggregationList = await _docCodeService.GetAggregationList(viewmodel.DocCodeConfig.AggrCompatibility);
                viewmodel.ExpeditionCompanies = await _docCodeService.GetExpeditionCompanies(viewmodel.DocCodeConfig.CompanyName);
                viewmodel.ExpeditionTypes = await _docCodeService.GetExpeditionTypes(viewmodel.DocCodeConfig.ExpeditionType);
                viewmodel.TreatmentTypes = await _docCodeService.GetTreatmentTypes(viewmodel.DocCodeConfig.TreatmentType);
                viewmodel.FinishingList = await _docCodeService.GetFinishingList(viewmodel.DocCodeConfig.Finishing);
                viewmodel.ArchiveList = await _docCodeService.GetArchiveList(viewmodel.DocCodeConfig.Archive);
                viewmodel.EmailList = await _docCodeService.GetEmailList(viewmodel.DocCodeConfig.Email);
                viewmodel.EmailHideList = await _docCodeService.GetEmailHideList(viewmodel.DocCodeConfig.EmailHide);
                viewmodel.ElectronicList = await _docCodeService.GetElectronicList(viewmodel.DocCodeConfig.Electronic);
                viewmodel.ElectronicHideList = await _docCodeService.GetElectronicHideList(viewmodel.DocCodeConfig.ElectronicHide);
                _logger.LogInfo("DocCodeException Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get ExceptoionDocCodeOptions action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        //TODO: DOCUMENT UNTESTED
        //TODO: HANDLE HTTP RESPONSES
        [HttpPost()]
        [ActionName("DocCodeConfig")]
        public async Task<ActionResult<DocCodeConfigViewModel>> AddDocCodeConfig([FromBody] DocCodeConfig model)
        {
            try
            {
                DocCodeConfigViewModel viewmodel = new DocCodeConfigViewModel();
                await _docCodeService.PostDocCodeConfig(model);
                List<DocCodeConfig> list = new List<DocCodeConfig>();
                list.Add(await _docCodeService.GetDocCodeConfig(model.DocCodeID, int.Parse(model.StartDate)));
                viewmodel.DocCodeConfigList = list;
                _logger.LogInfo("DocCodeException Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get ExceptoionDocCodeOptions action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        //TODO: DOCUMENT UNTESTED
        //TODO: HANDLE HTTP RESPONSES
        [HttpDelete("{ID}")]
        [ActionName("DocCode")]
        public async Task<ActionResult<DocCodeResultsViewModel>> DeleteDocCode([FromRoute] string ID)
        {
            try
            {
                DocCodeResultsViewModel viewmodel = new DocCodeResultsViewModel();
                viewmodel.Results = await _docCodeService.DeleteDocCode(ID);
                _logger.LogInfo("DocCodeException Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get ExceptoionDocCodeOptions action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }
        //Podia se mandar um aviso sobre algo nao ser compativel no futuro
        [HttpGet("{ID}")]
        [ActionName("Compatibility")]
        public async Task<ActionResult<DocCodeCompatabilityViewModel>> CompatibilityOptions([FromRoute] string ID)
        {
            try
            {
                DocCodeCompatabilityViewModel viewmodel = new DocCodeCompatabilityViewModel();
                viewmodel.DocCode = await _docCodeService.GetAggregateDocCode(ID);
                viewmodel.DocCodeList = await _docCodeService.GetAggregateDocCodes(ID);
                _logger.LogInfo("DocCodeException Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get ExceptoionDocCodeOptions action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPut()]
        [ActionName("Compatibility")]
        public async Task<ActionResult<DocCodeResultsViewModel>> DeleteDocCode([FromBody] DocCodeCompatabilityViewModel model)
        {
            try
            {
                await _docCodeService.ChangeCompatibility(model);
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
