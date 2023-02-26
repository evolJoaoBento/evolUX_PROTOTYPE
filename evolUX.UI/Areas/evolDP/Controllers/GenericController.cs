using Microsoft.AspNetCore.Mvc;
using System.Data;
using Flurl.Http;
using evolUX.UI.Exceptions;
using Newtonsoft.Json;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.evolDP;
using Shared.Models.Areas.evolDP;
using Microsoft.Extensions.Localization;
using System.Globalization;
using evolUX.UI.Areas.evolDP.Services.Interfaces;

namespace evolUX.UI.Areas.evolDP.Controllers
{
    [Area("evolDP")]
    public class GenericController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IGenericService _genericService;
        private readonly IStringLocalizer<GenericController> _localizer;
        public GenericController(IGenericService genericService, IStringLocalizer<GenericController> localizer, IConfiguration configuration)
        {
            _genericService = genericService;
            _localizer = localizer;
            _configuration = configuration;
        }

        public async Task<IActionResult> Projects()
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
                BusinessViewModel result = await _genericService.GetCompanyBusiness(CompanyBusinessList);
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
                        return RedirectToAction("ProjectList", new { CompanyBusinessValues = scValues });
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

        public async Task<IActionResult> ProjectList(int businessID)
        {
            try
            {
                string CompanyBusinessList = HttpContext.Session.GetString("evolDP/CompanyBusiness"); 
                DataTable CompanyBusinessDT = JsonConvert.DeserializeObject<DataTable>(HttpContext.Session.GetString("evolDP/CompanyBusiness"));

                ViewBag.OnlyOneSelected = true;
                if (businessID != 0)
                {
                    CompanyBusinessList = JsonConvert.SerializeObject(CompanyBusinessDT.Select(string.Format("[ID] = {0}", businessID)).CopyToDataTable());
                }


                ProjectListViewModel result = await _genericService.GetProjects(CompanyBusinessList);
 
                if (CompanyBusinessDT.Rows.Count > 1)
                {
                    ViewBag.hasMultipleCompanyBusiness = true;
                    if (businessID == 0) { ViewBag.OnlyOneSelected = false; }
                }
                else
                {
                    ViewBag.hasMultipleCompanyBusiness = false;
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

        public async Task<IActionResult> ConstantParameter()
        {
            try
            {
                ConstantParameterViewModel result = await _genericService.GetParameters();
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

        public async Task<IActionResult> ConfigParameter(IFormCollection form, string source)
        {
            try
            {
                int parameterID = 0;
                int parameterValue = 0;
                string strValue = form["ParameterID"].ToString();
                string parameterRef = form["ParameterRef"].ToString().Trim();
                string parameterDescription = form["ParameterDescription"].ToString().Trim();
                if (!string.IsNullOrEmpty(strValue) && !Int32.TryParse(strValue, out parameterID))
                    parameterID = 0;
                strValue = form["ParameterValue"].ToString();
                if (!string.IsNullOrEmpty(strValue) && !Int32.TryParse(strValue, out parameterValue))
                    parameterValue = 0;

                ConstantParameterViewModel result = await _genericService.SetParameter(parameterID, parameterRef, parameterValue, parameterDescription);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ConstantParameter", result);
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

        public async Task<IActionResult> DeleteParameter(int parameterID)
        {
            try
            {
                ConstantParameterViewModel result;
                if (parameterID != 0)
                    result = await _genericService.DeleteParameter(parameterID);
                else
                    result = await _genericService.GetParameters();
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));

                return View("ConstantParameter", result);
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
