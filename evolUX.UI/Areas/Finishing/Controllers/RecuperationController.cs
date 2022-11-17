using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.General;
using Shared.ViewModels.Areas.Finishing;
using evolUX.UI.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using Shared.Models.Areas.Core;

namespace evolUX.UI.Areas.EvolDP.Controllers
{
    [Area("Finishing")]
    public class RecuperationController : Controller
    {
        private readonly IRecuperationService _recuperationService;
        public RecuperationController(IRecuperationService recuperationService)
        {
            _recuperationService = recuperationService;
        }

        public ActionResult RegistTotalRecover()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistTotalRecover(string FileBarcode)
        {
            DataTable ServiceCompanyList = HttpContext.Session.Get<DataTable>("evolDP/ServiceCompanies");
            String user = HttpContext.Session.Get<AuthenticateResponse>("UserInfo").Username;
            bool PermissionLevel = HttpContext.Session.Get<bool>("PermissionLevel");

            var response = await _recuperationService.RegistTotalRecover(FileBarcode, user, ServiceCompanyList, PermissionLevel);
            if (response.StatusCode == ((int)HttpStatusCode.NotFound))
            {
                var resultError = response.GetJsonAsync<ErrorResult>().Result;
            }
            if (response.StatusCode == ((int)HttpStatusCode.Unauthorized))
            {
                if (response.Headers.Contains("Token-Expired"))
                {
                    var header = response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }

            ResultsViewModel result = await response.GetJsonAsync<ResultsViewModel>();
            return View("ResponsePartialView", result);
        }

        public ActionResult RegistPartialRecover()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistPartialRecover(string StartBarcode, string EndBarcode)
        {
            DataTable ServiceCompanyList = HttpContext.Session.Get<DataTable>("evolDP/ServiceCompanies");
            String user = HttpContext.Session.Get<AuthenticateResponse>("UserInfo").Username;
            bool PermissionLevel = HttpContext.Session.Get<bool>("PermissionLevel");

            var response = await _recuperationService.RegistPartialRecover(StartBarcode, EndBarcode, user, ServiceCompanyList, PermissionLevel);
            if (response.StatusCode == ((int)HttpStatusCode.NotFound))
            {
                var resultError = response.GetJsonAsync<ErrorResult>().Result;
            }
            if (response.StatusCode == ((int)HttpStatusCode.Unauthorized))
            {
                if (response.Headers.Contains("Token-Expired"))
                {
                    var header = response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }

            ResultsViewModel result = await response.GetJsonAsync<ResultsViewModel>();
            return View("ResponsePartialView", result);
        }

        public ActionResult RegistDetailRecover()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistDetailRecover(string StartBarcode, string EndBarcode)
        {
            DataTable ServiceCompanyList = HttpContext.Session.Get<DataTable>("evolDP/ServiceCompanies");
            String user = HttpContext.Session.Get<AuthenticateResponse>("UserInfo").Username;
            bool PermissionLevel = HttpContext.Session.Get<bool>("PermissionLevel");

            var response = await _recuperationService.RegistDetailRecover(StartBarcode, EndBarcode, user, ServiceCompanyList, PermissionLevel);
            if (response.StatusCode == ((int)HttpStatusCode.NotFound))
            {
                var resultError = response.GetJsonAsync<ErrorResult>().Result;
            }
            if (response.StatusCode == ((int)HttpStatusCode.Unauthorized))
            {
                if (response.Headers.Contains("Token-Expired"))
                {
                    var header = response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }

            ResultsViewModel result = await response.GetJsonAsync<ResultsViewModel>();
            return View("ResponsePartialView", result);
        }

        //THIS METHOD COULD BE BETTER IF IT WAS CALLED ASYNCRONOUSLY EACH TIME IN AN AJAX REQUEST
        public async Task<IActionResult> PendingRecover()
        {
            DataTable ServiceCompanyList = HttpContext.Session.Get<DataTable>("evolDP/ServiceCompanies");
            List<PendingRecoveriesViewModel> PendingRecoveries = null;
            foreach(DataRow row in ServiceCompanyList.Rows)
            {
                int i = (int)row["ID"];
                var response = await _recuperationService.GetPendingRecoveries(i);
                if (response.StatusCode == ((int)HttpStatusCode.NotFound))
                {
                    var resultError = response.GetJsonAsync<ErrorResult>().Result;
                }
                if (response.StatusCode == ((int)HttpStatusCode.Unauthorized))
                {
                    if (response.Headers.Contains("Token-Expired"))
                    {
                        var header = response.Headers.FirstOrDefault("Token-Expired");
                        var returnUrl = Request.Path.Value;
                        //var url = Url.RouteUrl("MyAreas", )

                        return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Auth", new { Area = "Core" });
                    }
                }

                PendingRecoveriesViewModel result = await response.GetJsonAsync<PendingRecoveriesViewModel>();
                PendingRecoveries = PendingRecoveries ?? new List<PendingRecoveriesViewModel>();
                PendingRecoveries.Add(result);
            }
           
            return View("PendingRecover", PendingRecoveries);
        }
        
        public async Task<IActionResult> PendingRecoveriesRegistDetail()
        {
            DataTable ServiceCompanyList = HttpContext.Session.Get<DataTable>("evolDP/ServiceCompanies");
            List<PendingRecoveriesViewModel> PendingRecoveries = null;
            foreach(DataRow row in ServiceCompanyList.Rows)
            {
                int i = (int)row["ID"];
                var response = await _recuperationService.GetPendingRecoveriesRegistDetail(i);
                if (response.StatusCode == ((int)HttpStatusCode.NotFound))
                {
                    var resultError = response.GetJsonAsync<ErrorResult>().Result;
                }
                if (response.StatusCode == ((int)HttpStatusCode.Unauthorized))
                {
                    if (response.Headers.Contains("Token-Expired"))
                    {
                        var header = response.Headers.FirstOrDefault("Token-Expired");
                        var returnUrl = Request.Path.Value;
                        //var url = Url.RouteUrl("MyAreas", )

                        return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Auth", new { Area = "Core" });
                    }
                }

                PendingRecoveriesViewModel result = await response.GetJsonAsync<PendingRecoveriesViewModel>();
                PendingRecoveries = PendingRecoveries ?? new List<PendingRecoveriesViewModel>();
                PendingRecoveries.Add(result);
            }

            return View("PendingRecoveriesRegistDetail", PendingRecoveries);
        }

    }
}
