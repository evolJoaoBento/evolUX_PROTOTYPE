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
using System.Security.Claims;
using Shared.ViewModels.Areas.evolDP;
using Shared.Models.Areas.evolDP;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using System.Globalization;

namespace evolUX.UI.Areas.Finishing.Controllers
{
    [Area("Finishing")]
    public class ExpeditionController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IExpeditionService _expeditionService;
        private readonly IStringLocalizer<ExpeditionController> _localizer;
        public ExpeditionController(IExpeditionService expeditionService, IStringLocalizer<ExpeditionController> localizer, IConfiguration configuration)
        {
            _expeditionService = expeditionService;
            _localizer = localizer;
            _configuration = configuration;
        }

        public async Task<IActionResult> PendingExpedition()
        {
            string cultureCode = CultureInfo.CurrentCulture.Name;
            string evolDP_DescriptionJSON = HttpContext.Session.GetString("evolDP/evolDP_DESCRIPTION");
            TempData["BusinessCode"] = "";
            if (!string.IsNullOrEmpty(evolDP_DescriptionJSON))
            {
                var evolDP_Desc = JsonConvert.DeserializeObject<List<dynamic>>(evolDP_DescriptionJSON);
                if (evolDP_Desc != null) 
                {
                    var b = evolDP_Desc.Find(x => x.FieldName == "BusinessCode" + "_" + cultureCode);
                    if (b == null) { b = evolDP_Desc.Find(x => x.FieldName == "BusinessCode" + "_" + cultureCode); }
                    if (b != null) { TempData["BusinessCode"] = b.FieldDescription; }
                }
            }
            string CompanyBusinessList = HttpContext.Session.GetString("evolDP/CompanyBusiness");
            try
            {
                if (string.IsNullOrEmpty(CompanyBusinessList))
                    return View(null);
                BusinessViewModel result = await _expeditionService.GetCompanyBusiness(CompanyBusinessList);
                if (result != null && result.CompanyBusiness.Count() > 0)
                {
                    if (result.CompanyBusiness.Count() > 1)
                    { 
                        List<Business> cList = result.CompanyBusiness.ToList();
                        string AllDesc = _localizer["All"];
                        cList.Add(new Business { BusinessID = 0, BusinessCode = "", Description = AllDesc });
                        result.CompanyBusiness = cList.OrderBy(x => x.BusinessID);
                        return View(result);
                    }
                    else
                    {
                        string scValues = result.CompanyBusiness.First().BusinessID + "|" + result.CompanyBusiness.First().BusinessCode + "|" + result.CompanyBusiness.First().Description;
                        return RedirectToAction("ExpeditionFileList", new { CompanyBusinessValues = scValues });
                    }
                }
                else
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

        public async Task<IActionResult> PendingExpeditionFiles(string CompanyBusinessValues)
        {
            try
            {
                string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
                if (string.IsNullOrEmpty(ServiceCompanyList))
                    return View(null);

                string[] companyBusinessValue = CompanyBusinessValues.Split('|');
                int BusinessID = Convert.ToInt32(companyBusinessValue[0]);
                string BusinessCode = companyBusinessValue.Length > 1 ? companyBusinessValue[1] : "";
                string BusinessDescription = companyBusinessValue.Length > 2 ? companyBusinessValue[2] : "";

                TempData["BusinessID"] = companyBusinessValue[0];
                TempData["BusinessCode"] = BusinessCode;
                TempData["BusinessDescription"] = BusinessDescription;

                ExpeditionListViewModel result = await _expeditionService.GetPendingExpeditionFiles(BusinessID, ServiceCompanyList);
                ViewBag.BusinessID = BusinessID;
                ViewBag.BusinessCode = BusinessCode;

                DataTable CompanyBusinessDT = JsonConvert.DeserializeObject<DataTable>(HttpContext.Session.GetString("evolDP/CompanyBusiness"));
                if (CompanyBusinessDT.Rows.Count > 1)
                {
                    ViewBag.hasMultipleCompanyBusiness = true;
                    ViewBag.BusinessDescription = "";
                    ViewBag.ExpeditionReportsJobs = 0;
                }
                else
                {
                    ViewBag.hasMultipleCompanyBusiness = false;
                    ViewBag.BusinessDescription = " [" + BusinessDescription + "]";
                    ViewBag.ExpeditionReportsJobs = 0;
                }
                DataTable ServiceCompanyDT = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyList);
                if (ServiceCompanyDT.Rows.Count > 1)
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

        public async Task<IActionResult> RegistExpeditionReport(List<string> CheckedFileList)
        {
            try
            {
                if (CheckedFileList == null || CheckedFileList.Count < 1)
                    return View(null);
                string username = User.Identity.Name;
                int userID = int.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                  .Select(c => c.Value)
                                                  .SingleOrDefault());

                List<RegistExpReportElement> expFiles = new List<RegistExpReportElement>();
                foreach (string cFile in CheckedFileList)
                {
                    string[] checkedFile = cFile.Split('|');
                    string expServiceCompanyCode = Convert.ToString(checkedFile[0]);
                    RegistExpReportElement? rElement = expFiles.FirstOrDefault(x => x.ServiceCompanyCode == expServiceCompanyCode);
                    if (rElement == null)
                    {
                        rElement = new RegistExpReportElement(expServiceCompanyCode);
                        expFiles.Add(rElement);
                    }
                    int expCompanyID = Convert.ToInt32(checkedFile[1]);
                    int runID = Convert.ToInt32(checkedFile[2]);
                    int fileID = Convert.ToInt32(checkedFile[3]);

                    rElement.ExpFileList.Add(new FileBase(runID, fileID));
                }
                Result result = await _expeditionService.RegistExpeditionReport(expFiles, username, userID);
                return PartialView("MessageView", new MessageViewModel(result.ErrorID.ToString(), "", result.Error));
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

        public async Task<IActionResult> Index()
        {
            string cultureCode = CultureInfo.CurrentCulture.Name;
            string evolDP_DescriptionJSON = HttpContext.Session.GetString("evolDP/evolDP_DESCRIPTION");
            TempData["BusinessCode"] = "";
            if (!string.IsNullOrEmpty(evolDP_DescriptionJSON))
            {
                var evolDP_Desc = JsonConvert.DeserializeObject<List<dynamic>>(evolDP_DescriptionJSON);
                if (evolDP_Desc != null)
                {
                    var b = evolDP_Desc.Find(x => x.FieldName == "BusinessCode" + "_" + cultureCode);
                    if (b == null) { b = evolDP_Desc.Find(x => x.FieldName == "BusinessCode" + "_" + cultureCode); }
                    if (b != null) { TempData["BusinessCode"] = b.FieldDescription; }
                }
            }
            string CompanyBusinessList = HttpContext.Session.GetString("evolDP/CompanyBusiness");
            try
            {
                if (string.IsNullOrEmpty(CompanyBusinessList))
                    return View(null);
                BusinessViewModel result = await _expeditionService.GetCompanyBusiness(CompanyBusinessList);
                if (result != null && result.CompanyBusiness.Count() > 0)
                {
                    if (result.CompanyBusiness.Count() > 1)
                    {
                        List<Business> cList = result.CompanyBusiness.ToList();
                        string AllDesc = _localizer["All"];
                        cList.Add(new Business { BusinessID = 0, BusinessCode = "", Description = AllDesc });
                        result.CompanyBusiness = cList.OrderBy(x => x.BusinessID);
                        return View(result);
                    }
                    else
                    {
                        string scValues = result.CompanyBusiness.First().BusinessID + "|" + result.CompanyBusiness.First().BusinessCode + "|" + result.CompanyBusiness.First().Description;
                        return RedirectToAction("ReportList", new { CompanyBusinessValues = scValues });
                    }
                }
                else
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

        public async Task<IActionResult> ReportList(string CompanyBusinessValues)
        {
            try
            {
                string expFolder = _configuration.GetValue<string>("ExpeditionFolder");
                ViewBag.ExpFolder = expFolder;

                string expeditionURL = _configuration.GetValue<string>("evolUXSiteURL");
                ViewBag.ExpeditionURL = expeditionURL + "/expedition/";

                string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
                if (string.IsNullOrEmpty(ServiceCompanyList))
                    return View(null);

                string[] companyBusinessValue = CompanyBusinessValues.Split('|');
                int BusinessID = Convert.ToInt32(companyBusinessValue[0]);
                string BusinessCode = companyBusinessValue.Length > 1 ? companyBusinessValue[1] : "";
                string BusinessDescription = companyBusinessValue.Length > 2 ? companyBusinessValue[2] : "";

                TempData["BusinessID"] = companyBusinessValue[0];
                TempData["BusinessCode"] = BusinessCode;
                TempData["BusinessDescription"] = BusinessDescription;

                ExpeditionListViewModel result = await _expeditionService.GetExpeditionReportList(BusinessID, ServiceCompanyList);
                ViewBag.BusinessID = BusinessID;
                ViewBag.BusinessCode = BusinessCode;

                DataTable CompanyBusinessDT = JsonConvert.DeserializeObject<DataTable>(HttpContext.Session.GetString("evolDP/CompanyBusiness"));
                if (CompanyBusinessDT.Rows.Count > 1)
                {
                    ViewBag.hasMultipleCompanyBusiness = true;
                    ViewBag.BusinessDescription = "";
                }
                else
                {
                    ViewBag.hasMultipleCompanyBusiness = false;
                    ViewBag.BusinessDescription = " [" + BusinessDescription + "]";
                }
                DataTable ServiceCompanyDT = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyList);
                if (ServiceCompanyDT.Rows.Count > 1)
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
