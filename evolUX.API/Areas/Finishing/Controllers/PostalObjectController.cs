using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using System.Data;
using Newtonsoft.Json;
using Shared.Models.General;

namespace evolUX.API.Areas.Finishing.Controllers
{
    [Route("api/finishing/[controller]/[action]")]
    [ApiController]
    public class PostalObjectController : ControllerBase
    {
        private readonly ILoggerService _logger;
        private readonly IPostalObjectService _postalObjectService;
        public PostalObjectController(IWrapperRepository repository, ILoggerService logger, IPostalObjectService postalObjectService)
        {
            _logger = logger;
            _postalObjectService = postalObjectService;
        }

        [HttpGet]
        [ActionName("GetPostalObjectInfo")]
        public async Task<ActionResult<ResultsViewModel>> GetPostalObjectInfo([FromBody] string ServiceCompanyListJSON, [FromQuery] string PostObjBarCode)
        {
            try
            {
                DataTable ServiceCompanyList = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyListJSON);

                PostalObjectViewModel viewmodel = await _postalObjectService.GetPostalObjectInfo(ServiceCompanyList, PostObjBarCode);
                _logger.LogInfo("PostalObjectInfo Get");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside Get Printers action: {ex.Message}");
                //return StatusCode(500, "Internal Server Error");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }
        
        //public async Task<ActionResult<ResultsViewModel>> GetPostalObjectInfo([FromBody] Dictionary<string, object> dictionary)
        //{
        //    try
        //    {
        //        object obj;
        //        dictionary.TryGetValue("ServiceCompanyList", out obj);
        //        DataTable ServiceCompanyList = JsonConvert.DeserializeObject<DataTable>(Convert.ToString(obj));
        //        dictionary.TryGetValue("PostObjBarCode", out obj);
        //        string PostObjBarCode = Convert.ToString(obj);

        //        PostalObjectViewModel viewmodel = await _postalObjectService.GetPostalObjectInfo(ServiceCompanyList, PostObjBarCode);
        //        _logger.LogInfo("PostalObjectInfo Get");
        //        return Ok(viewmodel);
        //    }
        //    catch (Exception ex)
        //    {
        //        //log error
        //        _logger.LogError($"Something went wrong inside Get Printers action: {ex.Message}");
        //        //return StatusCode(500, "Internal Server Error");
        //        return Problem(
        //            detail: ex.StackTrace,
        //            title: ex.Message);
        //    }
        //}

    }
}
