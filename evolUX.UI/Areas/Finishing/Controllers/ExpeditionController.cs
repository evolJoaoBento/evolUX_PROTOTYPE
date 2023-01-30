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

namespace evolUX.UI.Areas.Finishing.Controllers
{
    [Area("Finishing")]
    public class ExpeditionController : Controller
    {
        private readonly IExpeditionService _expeditionService;
        private readonly IStringLocalizer<RecoverController> _localizer;
        public ExpeditionController(IExpeditionService expeditionService, IStringLocalizer<RecoverController> localizer)
        {
            _expeditionService = expeditionService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            string CompanyBusinessList = HttpContext.Session.GetString("evolDP/CompanyBusiness");
            try
            {
                if (string.IsNullOrEmpty(CompanyBusinessList))
                    return View(null);
                BusinessViewModel result = await _expeditionService.GetCompanyBusiness(CompanyBusinessList);
                if (result != null && result.CompanyBusiness.Count() > 1)
                {
                    string AllDesc = _localizer["All"];
                    result.CompanyBusiness.Append(new Business { BusinessID = 0, BusinessCode = "", Description = AllDesc });
                    result.CompanyBusiness.OrderBy(x => x.BusinessID);
                    return View(result);
                }
                else if (result != null)
                {
                    string scValues = result.CompanyBusiness.First().BusinessID + "|" + result.CompanyBusiness.First().BusinessCode + "|" + result.CompanyBusiness.First().Description;
                    return RedirectToAction("ExpeditionFileList", new { CompanyBusinessValues = scValues });
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

        public async Task<IActionResult> ExpeditionFileList(string CompanyBusinessValues)
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

                ExpeditionFilesViewModel result = await _expeditionService.GetPendingExpeditionFiles(BusinessID, ServiceCompanyList);
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
