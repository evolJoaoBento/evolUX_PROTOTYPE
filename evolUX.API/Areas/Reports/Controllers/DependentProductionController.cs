﻿using Microsoft.AspNetCore.Http;
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
using evolUX.API.Areas.Reports.Services;

namespace evolUX.API.Areas.Reports.Controllers
{
    [Route("api/reports/DependentProduction/[action]")]
    [ApiController]
    public class DependentProductionController : ControllerBase
    {
        private readonly IWrapperRepository _repository;
        private readonly ILoggerService _logger;
        private readonly IDependentProductionService _dependentProductionService;
        public DependentProductionController(IWrapperRepository repository, ILoggerService logger, IDependentProductionService dependentProductionService)
        {
            _repository = repository;
            _logger = logger;
            _dependentProductionService = dependentProductionService;
        }
    }
}
