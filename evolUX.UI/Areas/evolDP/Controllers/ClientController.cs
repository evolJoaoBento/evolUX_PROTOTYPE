﻿using Microsoft.AspNetCore.Mvc;
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
    public class ClientController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IClientService _projectService;
        private readonly IStringLocalizer<ClientController> _localizer;
        public ClientController(IClientService projectService, IStringLocalizer<ClientController> localizer, IConfiguration configuration)
        {
            _projectService = projectService;
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
                BusinessViewModel result = await _projectService.GetCompanyBusiness(CompanyBusinessList);
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


                ProjectListViewModel result = await _projectService.GetProjects(CompanyBusinessList);
 
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
    }
}
