using Shared.ViewModels.Areas.Finishing;
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

namespace evolUX.UI.Areas.EvolDP.Controllers
{
    [Area("Finishing")]
    public class PendingRegistController : Controller
    {
        private readonly IPendingRegistService _pendingRegistService;
        public PendingRegistController(IPendingRegistService pendingRegistService)
        {
            _pendingRegistService = pendingRegistService;
        }

        public async Task<IActionResult> PendingRegist()
        {
            string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
            try
            {
                PendingRegistViewModel result = await _pendingRegistService.GetPendingRegist(ServiceCompanyList);
                DataTable ServiceCompanyDT = JsonConvert.DeserializeObject<DataTable>(HttpContext.Session.GetString("evolDP/ServiceCompanies"));
                if (ServiceCompanyDT.Rows.Count>1)
                {
                    ViewBag.hasMultipleServiceCompanies = true;
                }
                else
                {
                    ViewBag.hasMultipleServiceCompanies = false;
                }
                return View(result);
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
        public async Task<IActionResult> PendingRegistDetail(int RunID, string RunName)
        {
            string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
            try
            {
                PendingRegistDetailViewModel result = await _pendingRegistService.GetPendingRegistDetail(RunID, ServiceCompanyList);
                DataTable ServiceCompanyDT = JsonConvert.DeserializeObject<DataTable>(HttpContext.Session.GetString("evolDP/ServiceCompanies"));
                if (ServiceCompanyDT.Rows.Count > 1)
                {
                    ViewBag.hasMultipleServiceCompanies = true;
                }
                else
                {
                    ViewBag.hasMultipleServiceCompanies = false;
                }
                ViewBag.RunName = RunName;
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
    }
}
