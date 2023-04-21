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
using evolUX.API.Models;
using System.ComponentModel.Design;
using System;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<IActionResult> Companies()
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
            try
            {
                string CompaniesList = HttpContext.Session.GetString("evolDP/Companies");
                DataTable dTable = JsonConvert.DeserializeObject<DataTable>(CompaniesList);
                if (dTable.Rows.Count > 1)
                {
                    CompaniesViewModel result = new CompaniesViewModel();
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    List<Company> cList = new List<Company>();
                    foreach (DataRow row in dTable.Rows)
                    {
                        cList.Add(new Company
                        {
                            ID = Int32.Parse(row["ID"].ToString()),
                            CompanyCode = (string)row["CompanyCode"],
                            CompanyName = (string)row["CompanyName"]
                        });
                    }
                    result.Companies = cList;

                    string CompanyBusinessList = HttpContext.Session.GetString("evolDP/CompanyBusiness");
                    dTable = JsonConvert.DeserializeObject<DataTable>(CompanyBusinessList);
                    List<Business> bList = new List<Business>();
                    foreach (DataRow row in dTable.Rows)
                    {
                        bList.Add(new Business
                        {
                            BusinessID = Int32.Parse(row["ID"].ToString()),
                            BusinessCode = (string)row["BusinessCode"],
                            Description = (string)row["Description"],
                            CompanyID = Int32.Parse(row["CompanyID"].ToString())
                        });
                    }
                    result.CompanyBusiness = bList;
                    return View(result);
                }
                else
                {
                    return RedirectToAction("Business", "Generic", new { Area = "evolDP", companyID = Int32.Parse(dTable.Rows[0]["ID"].ToString()) });
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

        public async Task<IActionResult> Business(int companyID, string source)
        {
            TempData["Source"] = source;
            string cultureCode = CultureInfo.CurrentCulture.Name;
            try
            {
                BusinessViewModel result = await _genericService.GetCompanyBusiness(companyID);
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

        public async Task<IActionResult> ChangeCompany(IFormCollection form)
        {
            try
            {
                Company company = new Company();

                int value = 0;
                string str;
                company.ID = 0;
                str = form["CompanyID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    company.ID = value;

                company.CompanyName = form["CompanyName"].ToString();
                company.CompanyAddress = form["CompanyAddress"].ToString();
                company.CompanyPostalCode = form["CompanyPostalCode"].ToString();
                company.CompanyPostalCodeDescription = form["CompanyPostalCodeDescription"].ToString();
                company.CompanyCountry = form["CompanyCountry"].ToString();
                company.CompanyCode = form["CompanyCode"].ToString();
                company.CompanyServer = form["CompanyServer"].ToString();

                company = await _genericService.SetCompany(company);
                BusinessViewModel result = await _genericService.GetCompanyBusiness(company.ID);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                return View("Business", result);
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

        public async Task<IActionResult> ChangeBusiness(int companyID, IFormCollection form)
        {
            try
            {
                Business business = new Business();

                int value = 0;
                string str;
                business.BusinessID = 0;
                str = form["BusinessID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    business.BusinessID = value;
                business.CompanyID = companyID;
                business.BusinessCode = form["BusinessCode"].ToString();
                business.Description = form["Description"].ToString();
                str = form["FileSheetsCutoffLevel"].ToString();
                business.FileSheetsCutoffLevel = 0;
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    business.FileSheetsCutoffLevel = value;
                business.InternalExpeditionMode = 0;
                string[] list = form["InternalExpeditionMode"];
                for(int i = 0; i < list.Length; i++)
                {
                    if (!string.IsNullOrEmpty(list[i]) && int.TryParse(list[i], out value))
                        business.InternalExpeditionMode += value;
                }
                business.InternalCodeStart = 0;
                str = form["InternalCodeStart"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    business.InternalCodeStart = value;

                business.InternalCodeLen = 0;
                str = form["InternalCodeLen"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    business.InternalCodeLen = value;

                business.ExternalExpeditionMode = 0;
                str = form["ExternalExpeditionMode"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    business.ExternalExpeditionMode = value;

                business.TotalBannerPages = 0;
                str = form["TotalBannerPages"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    business.TotalBannerPages = value;

                business.PostObjOrderMode = 0;
                str = form["PostObjOrderMode"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    business.PostObjOrderMode = value;

                await _genericService.SetBusiness(business);
                BusinessViewModel result = await _genericService.GetCompanyBusiness(business.CompanyID);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                return View("Business", result);
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
        
        public async Task<IActionResult> Projects()
        {
            string CompaniesList = HttpContext.Session.GetString("evolDP/Companies");
            try
            {
                if (string.IsNullOrEmpty(CompaniesList))
                    return View(null);
                BusinessViewModel result = await _genericService.GetCompanyBusiness(CompaniesList);
                if (result != null && result.CompanyBusiness.Count() > 0)
                {
                    if (result.CompanyBusiness.Count() > 1)
                        return View(result);
                    else
                    {
                        string scValues = result.CompanyBusiness.First().BusinessID + "|" + result.CompanyBusiness.First().BusinessCode + "|" + result.CompanyBusiness.First().Description;
                        return RedirectToAction("ProjectList", "Generic", new { Area = "evolDP", CompanyBusinessValues = scValues });
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
                DataTable CompanyBusinessDT = JsonConvert.DeserializeObject<DataTable>(CompanyBusinessList);

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
