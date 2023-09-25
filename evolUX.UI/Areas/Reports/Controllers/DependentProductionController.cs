using Microsoft.AspNetCore.Mvc;
using System.Data;
using Flurl.Http;
using evolUX.UI.Exceptions;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Microsoft.Extensions.Localization;
using Shared.ViewModels.Areas.Reports;
using evolUX.UI.Areas.Reports.Services.Interfaces;

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

        public async Task<IActionResult> Index(int RunID, string ServiceCompanyList)
        {
            try
            {
                List<int> serviceCompanyList = new List<int>();
                string[] serviceCompanyListStr = ServiceCompanyList.Split('|');
                if (string.IsNullOrEmpty(ServiceCompanyList) || serviceCompanyListStr.Length == 0)
                {
                    return PartialView("MessageView", new MessageViewModel(_localizer["Missing Runs"]));
                }
                foreach (string s in serviceCompanyListStr)
                    serviceCompanyList.Add(int.Parse(s));

                DependentProductionViewModel result = await _dependentProductionService.GetDependentPrintsProduction(RunID, serviceCompanyList);

                if (result != null && result.DependentPrintProduction != null && result.DependentPrintProduction.Count() > 0)
                {
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                }
                return View(result);
            }
            catch (FlurlHttpException ex)
            {

                var resultError = await ex.GetResponseJsonAsync<ErrorResult>();
                return View("Error", resultError);
            }
            catch (HttpNotFoundException ex)
            {
                var resultError = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", resultError);
            }
            catch (HttpUnauthorizedException ex)
            {
                if (ex.response.Headers.Contains("Token-Expired"))
                {
                    var header = ex.response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;

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
