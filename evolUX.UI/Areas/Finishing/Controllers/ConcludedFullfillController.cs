using Shared.ViewModels.General;
using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Data;
using evolUX.UI.Extensions;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Microsoft.Extensions.Localization;
using evolUX.UI.Exceptions;
using Shared.Exceptions;
using Newtonsoft.Json;

namespace evolUX.UI.Areas.Finishing.Controllers
{
    [Area("Finishing")]
    public class ConcludedFullfillController : Controller
    {
        private readonly IConcludedFullfillService _concludedFullfillService;
        private readonly IStringLocalizer<ConcludedFullfillController> _localizer;
        public ConcludedFullfillController(IConcludedFullfillService concludedFullfillService, IStringLocalizer<ConcludedFullfillController> localizer)
        {
            _concludedFullfillService = concludedFullfillService;
            _localizer = localizer;
        }

        public ActionResult Index()
        {
            string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
            if (string.IsNullOrEmpty(ServiceCompanyList))
            {
                ViewBag.hasServiceCompanies = false;
            }
            else
            {
                ViewBag.hasServiceCompanies = true;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistFullFill(string FileBarcode)
        {
            string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
            string user = HttpContext.Session.Get<AuthenticateResponse>("UserInfo").Username;

            try
            {
                ResultsViewModel result = await _concludedFullfillService.RegistFullFill(FileBarcode, user, ServiceCompanyList);
                return PartialView("MessageView", new MessageViewModel("0", "", _localizer[result.Results.Error]));
            }
            catch (ControledErrorException ex)
            {
                return PartialView("MessageView", ex.ControledMessage);
            }
            catch (ErrorViewModelException ex)
            {
                return PartialView("Error", ex.ViewModel);
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
