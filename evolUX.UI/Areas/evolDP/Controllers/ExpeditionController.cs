using evolUX.UI.Areas.evolDP.Services.Interfaces;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.evolDP;
using System.Data;
using System.Globalization;
using System.Net;

namespace evolUX.UI.Areas.evolDP.Controllers
{
    [Area("evolDP")]
    public class ExpeditionController : Controller
    {
        private readonly IExpeditionService _expeditionService;
        public ExpeditionController(IExpeditionService expeditionService)
        {
            _expeditionService = expeditionService;
        }

        public async Task<IActionResult> Types()
        {
            try
            {
                string expCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");
                ExpeditionTypeViewModel result = await _expeditionService.GetExpeditionTypes(null, expCompanyList);
                if (result != null && result.Types != null && result.Types.Count() > 0)
                {
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    result.ExpCompanies = JsonConvert.DeserializeObject<List<Company>>(expCompanyList);
                    return View(result);
                }
                return View(null);
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
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
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
        public async Task<IActionResult> TypeDetail(string expeditionTypeJson, int expCompanyID)
        {
            try
            {
                ExpeditionTypeViewModel result = new ExpeditionTypeViewModel();
                if (string.IsNullOrEmpty(expeditionTypeJson) && expCompanyID > 0)
                {
                    result = await _expeditionService.GetExpCompanyTypes(null, expCompanyID);
                    ViewBag.SpecificExpCompany = true;
                }
                else
                {
                    ExpeditionTypeElement expeditionType = JsonConvert.DeserializeObject<ExpeditionTypeElement>(expeditionTypeJson);
                    List<ExpeditionTypeElement> types = new List<ExpeditionTypeElement>();
                    types.Add(expeditionType);
                    result.Types = types;

                    ViewBag.SpecificExpCompany = false;
                }
                string expCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                result.ExpCompanies = JsonConvert.DeserializeObject<List<Company>>(expCompanyList);

                return View("TypeDetail",result);
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
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
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

        public async Task<IActionResult> ChangeExpCompanyType(IFormCollection form, string expeditionTypeJson, bool specificExpCompany)
        {
            try
            {
                bool registMode = false;
                bool separationMode = false;
                bool barcodeRegistMode = false;

                string registModeStr = form["RegistMode"].ToString();
                string separationModeStr = form["SeparationMode"].ToString();
                int barcodeRegistModeValue = Int32.Parse(form["BarcodeRegistMode"].ToString());
                if (registModeStr == "on")
                    registMode = true;
                if (separationModeStr == "on")
                    separationMode = true;
                if (barcodeRegistModeValue == 1)
                    barcodeRegistMode = true;

                ExpeditionTypeViewModel result = new ExpeditionTypeViewModel();
                ExpeditionTypeElement expeditionType = JsonConvert.DeserializeObject<ExpeditionTypeElement>(expeditionTypeJson);
                result = await _expeditionService.SetExpCompanyType(expeditionType.ExpeditionType, expeditionType.ExpCompanyTypesList.First().ExpCompanyID, registMode, separationMode, barcodeRegistMode, specificExpCompany);

                string expCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                result.ExpCompanies = JsonConvert.DeserializeObject<List<Company>>(expCompanyList);
                ViewBag.SpecificExpCompany = specificExpCompany;
                return View("TypeDetail", result);
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
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
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

        public async Task<IActionResult> Zones()
        {
            try
            {
                string expCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");
                ExpeditionZoneViewModel result = await _expeditionService.GetExpeditionZones(null, expCompanyList);
                if (result != null && result.Zones != null && result.Zones.Count() > 0)
                {
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    result.ExpCompanies = JsonConvert.DeserializeObject<List<Company>>(expCompanyList);
                    return View(result);
                }
                return View(null);
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
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                return View("Error", viewModel);
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
