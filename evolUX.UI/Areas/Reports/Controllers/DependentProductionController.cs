using Microsoft.AspNetCore.Mvc;
using System.Data;
using Flurl.Http;
using evolUX.UI.Exceptions;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Microsoft.Extensions.Localization;
using Shared.ViewModels.Areas.Reports;
using evolUX.UI.Areas.Reports.Services.Interfaces;
using Newtonsoft.Json;

namespace evolUX.UI.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class DependentProductionController : Controller
    {
        private readonly IDependentProductionService _dependentProductionService;
        private readonly IStringLocalizer<DependentProductionController> _localizer;

        public DependentProductionController(IDependentProductionService dependentProductionService, IStringLocalizer<DependentProductionController> localizer)
        {
            _dependentProductionService = dependentProductionService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
            try
            {
                DataTable ServiceCompanies = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyList);
                DependentProductionViewModel result = await _dependentProductionService.GetDependentPrintsProduction(ServiceCompanies);

                if (result != null && result.DependentPrintProduction != null && result.DependentPrintProduction.Count() > 0)
                {
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                }
                return View(result);
            }
            catch (ErrorViewModelException ex)
            {
                return View("Error", ex.ViewModel);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }
    }
}
