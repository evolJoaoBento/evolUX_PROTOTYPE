﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using evolUX.API.Areas.Core.Services.Interfaces;
//using Shared.Models.Areas.Finishing;
using Shared.ViewModels.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using Shared.Models.Areas.Finishing;
using System.Data;
using Shared.BindingModels.Finishing;

namespace evolUX.API.Areas.Finishing.Controllers
{
    [Route("api/finishing/[controller]/[action]")]
    [ApiController]
    public class ConcludedEnvelopeController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IConcludedEnvelopeRepository _concludedEnvelopeService;
        public ConcludedEnvelopeController(IWrapperRepository repository, ILoggerService logger, IConcludedEnvelopeRepository concludedEnvelopeService)
        {
            _repository = repository;
            _logger = logger;
            _concludedEnvelopeService = concludedEnvelopeService;
        }


        //THE SERVICECOMPANYLIST SHOULD USE A SESSION VARIABLE IN THE UI LAYER
        [HttpPost]
        [ActionName("RegistFullFill")]
        public async Task<ActionResult<ResultsViewModel>> RegistFullFill([FromBody] Regist bindingModel)
        {
            try
            {
                ResultsViewModel viewmodel = new ResultsViewModel();
                viewmodel.Results = await _concludedEnvelopeService.RegistFullFill(bindingModel.FileBarcode, bindingModel.User, bindingModel.ServiceCompanyList);
                _logger.LogInfo("RegistFullFill Post");
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError($"Something went wrong inside RegistFullFill Post action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
