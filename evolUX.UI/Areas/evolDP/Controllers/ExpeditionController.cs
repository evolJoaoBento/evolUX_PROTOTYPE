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
using System.ComponentModel.Design;
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
        
        public async Task<IActionResult> ChangeExpCompany(IFormCollection form, string expeditionTypeViewJson)
        {
            try
            {
                ExpeditionTypeViewModel result = JsonConvert.DeserializeObject<ExpeditionTypeViewModel>(expeditionTypeViewJson);
                Company expCompany = new Company();

                int value = 0;
                string str;
                expCompany.ID = 0;
                str = form["ExpCompanyID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expCompany.ID = value;

                expCompany.CompanyName = form["CompanyName"].ToString();
                expCompany.CompanyAddress = form["CompanyAddress"].ToString();
                expCompany.CompanyPostalCode = form["CompanyPostalCode"].ToString();
                expCompany.CompanyPostalCodeDescription = form["CompanyPostalCodeDescription"].ToString();
                expCompany.CompanyCountry = form["CompanyCountry"].ToString();
                expCompany.CompanyCode = form["CompanyCode"].ToString();
                expCompany.CompanyServer = form["CompanyServer"].ToString();

                List<Company>  expCompanies = new List<Company>();
                expCompany = await _expeditionService.SetExpCompany(expCompany);
                expCompanies.Add(expCompany);
                result.ExpCompanies = expCompanies;
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ExpCompany", result);
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
                ExpCompanyViewModel result = new ExpCompanyViewModel();
                ExpeditionTypeViewModel expTypes;
                if (!string.IsNullOrEmpty(expeditionTypeViewJson))
                {
                    expTypes = JsonConvert.DeserializeObject<ExpeditionTypeViewModel>(expeditionTypeViewJson);
                }
                else
                {
                    DataTable expCompanyListDT = new DataTable();
                    expCompanyListDT.Columns.Add("ID", typeof(int));
                    DataRow row = expCompanyListDT.NewRow();
                    row["ID"] = expCompanyID;
                    expCompanyListDT.Rows.Add(row);
                    string expCompanyList = JsonConvert.SerializeObject(expCompanyListDT);

                    expTypes = await _expeditionService.GetExpeditionCompanies(expCompanyList);
                }
                result.ExpCompany = expTypes.ExpCompanies.Where(x => x.ID == expCompanyID).First();
                result.Types = expTypes.Types;
                result.Configs = await _expeditionService.GetExpCompanyConfigs(expCompanyID, 0, 0);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ExpCompany", result);
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

        public async Task<IActionResult> ExpCompanyConfig(string expCompanyViewModelJson, int startDate, int expeditionType, int expeditionZone)
        {
            try
            {
                ExpCompanyViewModel request = JsonConvert.DeserializeObject<ExpCompanyViewModel>(expCompanyViewModelJson);
                ExpCompanyConfigViewModel result = new ExpCompanyConfigViewModel();
                result.expCompanyView = request;
                result.StartDate = startDate;
                result.ExpeditionType = expeditionType;
                result.ExpeditionZone = expeditionZone;
                if (expeditionZone == 0)
                {
                    ExpeditionZoneViewModel zones = await _expeditionService.GetExpeditionZones(0, "");
                    result.Zones = zones.Zones.ToList();
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

        public async Task<IActionResult> ChangeExpCompanyConfig(IFormCollection form, string expCompanyJson)
        {
            try
            {
                Company company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
                int value = 0;
                string str;
                ExpCompanyConfig expCompanyConfig = new ExpCompanyConfig();
                expCompanyConfig.ExpCompanyID = 0;
                str = form["ExpCompanyID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expCompanyConfig.ExpCompanyID = value;

                expCompanyConfig.ExpeditionType = 0;
                str = form["ExpeditionType"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expCompanyConfig.ExpeditionType = value;

                expCompanyConfig.ExpeditionZone = 0;
                str = form["ExpeditionZone"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expCompanyConfig.ExpeditionZone = value;

                expCompanyConfig.ExpCompanyLevel = 0;
                str = form["ExpCompanyLevel"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expCompanyConfig.ExpCompanyLevel = value;

                DateTime startDateDT = DateTime.Now;
                expCompanyConfig.StartDate = 0;
                str = form["StartDate"].ToString();
                if (!string.IsNullOrEmpty(str) && DateTime.TryParse(str, out startDateDT))
                    expCompanyConfig.StartDate = Int32.Parse(((DateTime)startDateDT).ToString("yyyyMMdd"));

                expCompanyConfig.MaxWeight = 0;
                str = form["MaxWeight"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expCompanyConfig.MaxWeight = value;

                double valDouble = 0;
                expCompanyConfig.ExpCompanyLevel = 0;
                str = form["UnitCost"].ToString();
                if (!string.IsNullOrEmpty(str) && double.TryParse(str, out valDouble))
                    expCompanyConfig.UnitCost = valDouble;

                str = form["ExpColumnA"].ToString();
                if (!string.IsNullOrEmpty(str))
                    expCompanyConfig.ExpColumnA = str;

                str = form["ExpColumnA"].ToString();
                if (!string.IsNullOrEmpty(str))
                    expCompanyConfig.ExpColumnA = str;

                str = form["ExpColumnB"].ToString();
                if (!string.IsNullOrEmpty(str))
                    expCompanyConfig.ExpColumnB = str;

                str = form["ExpColumnE"].ToString();
                if (!string.IsNullOrEmpty(str))
                    expCompanyConfig.ExpColumnE = str;

                str = form["ExpColumnI"].ToString();
                if (!string.IsNullOrEmpty(str))
                    expCompanyConfig.ExpColumnI = str;

                str = form["ExpColumnF"].ToString();
                if (!string.IsNullOrEmpty(str))
                    expCompanyConfig.ExpColumnF = str;
      

                Company Company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
                ExpCompanyConfigViewModel result = new ExpCompanyConfigViewModel();
                result.expCompanyView.Configs = await _expeditionService.SetExpCompanyConfig(expCompanyConfig);
                result.expCompanyView.ExpCompany = company;
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                return View("ExpCompanyConfig", result);
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

        public async Task<IActionResult> ConfigExpRegistRange(IFormCollection form, string expCompanyJson)
        {
            try
            {
                int value = 0;
                string str;
                ExpeditionRegistElement expRegist = new ExpeditionRegistElement();
                expRegist.ExpCompanyID = 0;
                str = form["ExpCompanyID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expRegist.ExpCompanyID = value;

                expRegist.CompanyRegistCode = 0;
                str = form["CompanyRegistCode"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expRegist.CompanyRegistCode = value;

                expRegist.StartExpeditionID = 0;
                str = form["StartExpeditionID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expRegist.StartExpeditionID = value;

                expRegist.EndExpeditionID = 0;
                str = form["EndExpeditionID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expRegist.EndExpeditionID = value;

                expRegist.RegistCodePrefix = form["RegistCodePrefix"].ToString();
                expRegist.RegistCodeSuffix = form["RegistCodeSuffix"].ToString();

                expRegist.LastExpeditionID = 0;
                str = form["LastExpeditionID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expRegist.LastExpeditionID = value;

                ExpeditionRegistViewModel result = new ExpeditionRegistViewModel();
                result.Company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
                await _expeditionService.SetExpeditionRegistID(expRegist);
                result.ExpeditionRegistIDs = await _expeditionService.GetExpeditionRegistIDs(result.Company.ID);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ExpRegistRange", result);
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

        public async Task<IActionResult> ExpContracts(string expCompanyJson)
        {
            try
            {
                ExpContractViewModel result = new ExpContractViewModel();
                result.Company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
                result.ExpeditionContracts = await _expeditionService.GetExpContracts(result.Company.ID);
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

        public async Task<IActionResult> ConfigExpContract(IFormCollection form, string expCompanyJson)
        {
            try
            {
                ExpContractElement expContract = new ExpContractElement();

                int value = 0;
                string str;
                expContract.ExpCompanyID = 0;
                str = form["ExpCompanyID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expContract.ExpCompanyID = value;

                expContract.ContractID = 0;
                str = form["ContractID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expContract.ContractID = value;

                expContract.ContractNr = 0;
                str = form["ContractNr"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expContract.ContractNr = value;

                expContract.ClientNr = 0;
                str = form["ClientNr"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    expContract.ClientNr = value;

                expContract.ClientName = form["ClientName"].ToString();
                expContract.ClientNIF = form["ClientNIF"].ToString();
                expContract.ClientAddress = form["ClientAddress"].ToString();
                expContract.ClientPostalCode = form["ClientPostalCode"].ToString();
                expContract.ClientPostalCodeDescription = form["ClientPostalCodeDescription"].ToString();
                expContract.ClientNIF = form["ClientNIF"].ToString();
                expContract.CompanyExpeditionCode = form["CompanyExpeditionCode"].ToString();

                expContract.PurchaseOrderNr = 0;
                str = form["PurchaseOrderNr"].ToString();
                decimal dec = 0;
                if (!string.IsNullOrEmpty(str) && decimal.TryParse(str, out dec))
                    expContract.PurchaseOrderNr = dec;


                ExpContractViewModel result = new ExpContractViewModel();
                result.Company = JsonConvert.DeserializeObject<Company>(expCompanyJson);
                await _expeditionService.SetExpContract(expContract);
                result.ExpeditionContracts = await _expeditionService.GetExpContracts(result.Company.ID);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ExpContracts", result);
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
