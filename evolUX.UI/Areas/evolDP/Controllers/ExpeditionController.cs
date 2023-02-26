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
using System.Data.Common;
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
        public async Task<IActionResult> Companies()
        {
            try
            {
                string expCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");
                ExpeditionTypeViewModel result = await _expeditionService.GetExpeditionCompanies(expCompanyList);
                if (result != null && result.ExpCompanies != null && result.ExpCompanies.Count() > 0)
                {
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
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
        public async Task<IActionResult> ExpCompany(int expCompanyID, string expeditionTypeViewJson)
        {
            try
            {
                ExpeditionTypeViewModel result;
                if (!string.IsNullOrEmpty(expeditionTypeViewJson))
                    result = JsonConvert.DeserializeObject<ExpeditionTypeViewModel>(expeditionTypeViewJson);
                else
                {
                    DataTable expCompanyListDT = new DataTable();
                    expCompanyListDT.Columns.Add("ID", typeof(int));
                    DataRow row = expCompanyListDT.NewRow();
                    row["ID"] = expCompanyID;
                    expCompanyListDT.Rows.Add(row);
                    string expCompanyList = JsonConvert.SerializeObject(expCompanyListDT);

                    result = await _expeditionService.GetExpeditionCompanies(expCompanyList);
                }
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

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
        public async Task<IActionResult> TypeDetail(string expeditionTypeJson)
        {
            try
            {
                ExpeditionTypeViewModel result = new ExpeditionTypeViewModel();
                ExpeditionTypeElement expeditionType = JsonConvert.DeserializeObject<ExpeditionTypeElement>(expeditionTypeJson);
                List<ExpeditionTypeElement> types = new List<ExpeditionTypeElement>();
                types.Add(expeditionType);
                result.Types = types;
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

        public async Task<IActionResult> ZoneDetail(string expeditionZoneJson)
        {
            try
            {
                ExpeditionZoneViewModel result = new ExpeditionZoneViewModel();
                ExpeditionZoneElement expeditionZone = JsonConvert.DeserializeObject<ExpeditionZoneElement>(expeditionZoneJson);
                List<ExpeditionZoneElement> zones = new List<ExpeditionZoneElement>();
                zones.Add(expeditionZone);
                result.Zones = zones;

                string expCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                result.ExpCompanies = JsonConvert.DeserializeObject<List<Company>>(expCompanyList);

                return View("ZoneDetail", result);
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

        public async Task<IActionResult> ExpRegistRange(string expCompanyJson)
        {
            try
            {
                ExpeditionRegistViewModel result = new ExpeditionRegistViewModel();
                result.Company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
                result.ExpeditionRegistIDs = await _expeditionService.GetExpeditionRegistIDs(result.Company.ID);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

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
