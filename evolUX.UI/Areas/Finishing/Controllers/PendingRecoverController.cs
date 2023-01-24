﻿using Shared.ViewModels.Areas.Finishing;
using evolUX.API.Areas.Core.ViewModels;
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
using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using System.Security.Claims;

namespace evolUX.UI.Areas.Finishing.Controllers
{
    [Area("Finishing")]
    public class PendingRecoverController : Controller
    {
        private readonly IPendingRecoverService _pendingRecoverService;
        public PendingRecoverController(IPendingRecoverService pendingRecoverService)
        {
            _pendingRecoverService = pendingRecoverService;
        }

        public async Task<IActionResult> Index()
        {
            string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
            try
            {
                ServiceCompanyViewModel result = await _pendingRecoverService.GetServiceCompanies(ServiceCompanyList);
                if (result != null && result.ServiceCompanies.Count() > 1)
                {
                    return View(result);
                }
                else
                {
                    string scValues = result.ServiceCompanies.First().CompanyID + "|" + result.ServiceCompanies.First().CompanyCode + "|" + result.ServiceCompanies.First().CompanyName;
                    return RedirectToAction("PendingRecoverDetail", new { ServiceCompanyValues = scValues });
                }
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch(HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
            }
            catch(HttpUnauthorizedException ex)
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

        public async Task<IActionResult> PendingRecoverDetail(string ServiceCompanyValues)
        {
            try
            {
                string[] serviceCompanyValue = ServiceCompanyValues.Split('|');
                int ServiceCompanyID = Convert.ToInt32(serviceCompanyValue[0]);
                string ServiceCompanyCode = serviceCompanyValue.Length > 1 ? serviceCompanyValue[1] : "";
                string ServiceCompanyName = serviceCompanyValue.Length > 2 ? serviceCompanyValue[2] : "";

                TempData["ServiceCompanyID"] = serviceCompanyValue[0];
                TempData["ServiceCompanyCode"] = ServiceCompanyCode;
                TempData["ServiceCompanyName"] = ServiceCompanyName;

                PendingRecoverDetailViewModel result = await _pendingRecoverService.GetPendingRecoveries(ServiceCompanyID, ServiceCompanyCode);
                ViewBag.ServiceCompanyID = ServiceCompanyID;
                ViewBag.ServiceCompanyCode = ServiceCompanyCode;

                DataTable ServiceCompanyDT = JsonConvert.DeserializeObject<DataTable>(HttpContext.Session.GetString("evolDP/ServiceCompanies"));
                if (ServiceCompanyDT.Rows.Count>1)
                {
                    ViewBag.hasMultipleServiceCompanies = true;
                    ViewBag.ServiceCompanyName = " [" + ServiceCompanyName + "]";

                }
                else
                {
                    ViewBag.hasMultipleServiceCompanies = false;
                    ViewBag.ServiceCompanyName = "";

                }

                return View(result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape

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
                    //var url = Url.RouteUrl("MyAreas", )

                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
        }

        public async Task<IActionResult> RegistPendingRecover(string RecoverType)
        {
            string ServiceCompanyName = (string)TempData["ServiceCompanyName"];
            string ServiceCompanyCode = (string)TempData["ServiceCompanyCode"];
            int ServiceCompanyID = Convert.ToInt32((string)TempData["ServiceCompanyID"]);

            int userid = int.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                              .Select(c => c.Value)
                                              .SingleOrDefault());
            try
            {
                Result result = await _pendingRecoverService.RegistPendingRecover(ServiceCompanyID, ServiceCompanyCode, RecoverType, userid);
                if (result != null && result.Error.ToUpper() == "SUCCESS")
                {
                    string scValues = ServiceCompanyID.ToString() + "|" + ServiceCompanyCode + "|" + ServiceCompanyName;
                    return RedirectToAction("PendingRecoverDetail", "PendingRecover", new { ServiceCompanyValues = scValues });
                }
                return View("MessageView", new MessageViewModel(result.ErrorID.ToString(), "", result.Error));
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
