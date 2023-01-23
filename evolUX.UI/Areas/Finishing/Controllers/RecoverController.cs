﻿using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.General;
using Shared.ViewModels.Areas.Finishing;
using evolUX.UI.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Microsoft.Extensions.Localization;
using evolUX.UI.Exceptions;
using Shared.Exceptions;

namespace evolUX.UI.Areas.Finishing.Controllers
{
    [Area("Finishing")]
    public class RecoverController : Controller
    {
        private readonly IRecoverService _recoverService;
        private readonly IStringLocalizer<RecoverController> _localizer;
        public RecoverController(IRecoverService recoverService, IStringLocalizer<RecoverController> localizer)
        {
            _recoverService = recoverService;
            _localizer = localizer;
        }

        public ActionResult RegistTotalRecover()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistTotalRecover(string FileBarcode)
        {
            string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
            string user = HttpContext.Session.Get<AuthenticateResponse>("UserInfo").Username;
            bool PermissionLevel = HttpContext.Session.Get<bool>("PermissionLevel");

            try
            {
                ResultsViewModel result = await _recoverService.RegistTotalRecover(FileBarcode, user, ServiceCompanyList, PermissionLevel);
                return PartialView("MessageView", new MessageViewModel("0", "", _localizer["RegistTotalRecover" + result.Results.Error]));
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

        public ActionResult RegistPartialRecover()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistPartialRecover(string StartBarcode, string EndBarcode)
        {
            string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
            string user = HttpContext.Session.Get<AuthenticateResponse>("UserInfo").Username;
            bool PermissionLevel = HttpContext.Session.Get<bool>("PermissionLevel");

            try
            {
                ResultsViewModel result = await _recoverService.RegistPartialRecover(StartBarcode, EndBarcode, user, ServiceCompanyList, PermissionLevel);
                return PartialView("MessageView", new MessageViewModel("0", "", _localizer["RegistPartialRecover" + result.Results.Error]));
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

        public ActionResult RegistDetailRecover()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistDetailRecover(string StartBarcode, string EndBarcode)
        {
            string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
            string user = HttpContext.Session.Get<AuthenticateResponse>("UserInfo").Username;
            bool PermissionLevel = HttpContext.Session.Get<bool>("PermissionLevel");

            try
            {
                ResultsViewModel result = await _recoverService.RegistDetailRecover(StartBarcode, EndBarcode, user, ServiceCompanyList, PermissionLevel);
                return PartialView("MessageView", new MessageViewModel("0", "", _localizer["RegistDetailRecover" + result.Results.Error]));
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

        //THIS METHOD COULD BE BETTER IF IT WAS CALLED ASYNCRONOUSLY EACH TIME IN AN AJAX REQUEST
        public async Task<IActionResult> PendingRecover()
        {
            DataTable ServiceCompanyList = HttpContext.Session.Get<DataTable>("evolDP/ServiceCompanies");
            List<PendingRecoveriesViewModel> PendingRecoveries = null;
            foreach(DataRow row in ServiceCompanyList.Rows)
            {
                int i = (int)row["ID"];
                var response = await _recoverService.GetPendingRecoveries(i);
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
                var response = await _recoverService.GetPendingRecoveriesRegistDetail(i);
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
