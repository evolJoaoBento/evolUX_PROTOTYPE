using Shared.ViewModels.Areas.Finishing;
using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Data;
using Flurl.Http;
using evolUX.UI.Exceptions;
using Newtonsoft.Json;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using Shared.Models.General;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using evolUX.API.Models;
using Shared.Models.Areas.evolDP;
using Shared.Models.Areas.Finishing;
using Microsoft.Extensions.Localization;
using evolUX_dev.Areas.evolDP.Models;
using Shared.ViewModels.Areas.Reports;
using evolUX.UI.Areas.Reports.Services.Interfaces;

namespace evolUX.UI.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class RecoveryReportController : Controller
    {
        private readonly IRecoveryReportService _recoveryReportService;
        private readonly IStringLocalizer<RecoveryReportController> _localizer;


        public RecoveryReportController(IRecoveryReportService recoveryReportService, IStringLocalizer<RecoveryReportController> localizer)
        {
            _recoveryReportService = recoveryReportService;
            _localizer = localizer;
        }
    }
}
