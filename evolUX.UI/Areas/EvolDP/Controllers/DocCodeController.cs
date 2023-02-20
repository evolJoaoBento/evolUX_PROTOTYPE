using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Areas.EvolDP.Services.Interfaces;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.evolDP;
using System.Data;
using System.Globalization;
using System.Net;
using System.Reflection;

namespace evolUX.UI.Areas.EvolDP.Controllers
{
    [Area("EvolDP")]
    public class DocCodeController : Controller
    {
        private readonly IDocCodeService _docCodeService;
        public DocCodeController(IDocCodeService docCodeService)
        {
            _docCodeService = docCodeService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                DocCodeViewModel result = await _docCodeService.GetDocCodeGroup();
                if (result != null && result.DocCodeList != null && result.DocCodeList.Count() > 0)
                {
                    DocCodeGroupViewModel docCodeGroup = new DocCodeGroupViewModel();
                    docCodeGroup.DocCodeList = new List<DocCodeGroup>();
                    List<DocCode> docList = new List<DocCode>();
                    string lastDocLayout = string.Empty;
                    foreach(DocCode docCode in result.DocCodeList.ToList()) 
                    {
                        if (docCode.DocLayout != lastDocLayout)
                        {
                            docCodeGroup.DocCodeList.Add(new DocCodeGroup(docCode.DocLayout));
                            docList = docCodeGroup.DocCodeList[docCodeGroup.DocCodeList.Count-1].DocCodes;
                            lastDocLayout = docCode.DocLayout;
                        }
                        docList.Add(docCode);
                    }
                    return View(docCodeGroup);
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

        public async Task<IActionResult> DocCode(string doccodeJson)
        {
            try
            {
                DocCode docCode = JsonConvert.DeserializeObject<DocCode>(doccodeJson);
                DocCodeViewModel result = await _docCodeService.GetDocCode(docCode.DocLayout, docCode.DocType);
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

        public async Task<IActionResult> DocCodeConfig(string doccodeJson)
        {
            try
            {
                DocCode docCode = JsonConvert.DeserializeObject<DocCode>(doccodeJson);
                DocCodeViewModel result = await _docCodeService.GetDocCodeConfig(docCode.DocCodeID);
                if (result != null)
                {
                    DocCode d = result.DocCodeList.First();
                    d.DocLayout = docCode.DocLayout;
                    d.DocType = docCode.DocType;
                    d.PrintMatchCode = docCode.PrintMatchCode;
                    d.DocDescription = docCode.DocDescription;
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

        public async Task<IActionResult> AddDocCode(string doccodeJson)
        {
            try
            {
                DocCode? docCode = null;
                if (!string.IsNullOrEmpty(doccodeJson))
                    docCode = JsonConvert.DeserializeObject<DocCode>(doccodeJson);
                DocCodeConfigOptionsViewModel result = await _docCodeService.GetDocCodeConfigOptions(docCode);
                string ExpCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");
                if (!string.IsNullOrEmpty(ExpCompanyList) && result != null)
                    result.ExpCompanies = JsonConvert.DeserializeObject<List<Company>>(ExpCompanyList);

                string evolDP_DescriptionJSON = HttpContext.Session.GetString("evolDP/evolDP_DESCRIPTION");
                string cultureCode = CultureInfo.CurrentCulture.Name;
                if (!string.IsNullOrEmpty(cultureCode))
                    cultureCode = cultureCode.Substring(0,2);
                TempData["ExceptionLevel1ID"] = "";
                TempData["ExceptionLevel2ID"] = "";
                TempData["ExceptionLevel3ID"] = "";
                foreach (GenericOptionValue option in result.SuportTypeList.List)
                    TempData[option.Code] = "";
                foreach (GenericOptionValue option in result.SuportTypeList.OptionList)
                    TempData[option.GroupCode] = "";
                if (!string.IsNullOrEmpty(evolDP_DescriptionJSON))
                {
                    var evolDP_Desc = JsonConvert.DeserializeObject<List<dynamic>>(evolDP_DescriptionJSON);
                    if (evolDP_Desc != null)
                    {
                        bool b = false;
                        var val = evolDP_Desc.Find(x => x.FieldName == "ExceptionLevel1ID" + "_" + cultureCode);
                        if (val == null) { val = evolDP_Desc.Find(x => x.FieldName == "ExceptionLevel1ID"); }
                        if (val != null) { TempData["ExceptionLevel1ID"] = val.FieldDescription; }

                        val = evolDP_Desc.Find(x => x.FieldName == "ExceptionLevel2ID" + "_" + cultureCode);
                        if (val == null) { val = evolDP_Desc.Find(x => x.FieldName == "ExceptionLevel2ID"); }
                        if (val != null) { TempData["ExceptionLevel2ID"] = val.FieldDescription; }

                        val = evolDP_Desc.Find(x => x.FieldName == "ExceptionLevel3ID" + "_" + cultureCode);
                        if (val == null) { val = evolDP_Desc.Find(x => x.FieldName == "ExceptionLevel3ID"); }
                        if (val != null) { TempData["ExceptionLevel3ID"] = val.FieldDescription; }

                        foreach (GenericOptionValue option in result.SuportTypeList.List)
                        {
                            val = evolDP_Desc.Find(x => x.FieldName == option.Code + "_" + cultureCode);
                            if (val == null) { val = evolDP_Desc.Find(x => x.FieldName == option.Code); }
                            if (val != null) { TempData[option.Code] = val.FieldDescription; }
                        }
                        foreach (GenericOptionValue option in result.SuportTypeList.OptionList)
                        { 
                            val = evolDP_Desc.Find(x => x.FieldName == option.GroupCode + "_" + cultureCode);
                            if (val == null) { val = evolDP_Desc.Find(x => x.FieldName == option.GroupCode); }
                            if (val != null) { TempData[option.GroupCode] = val.FieldDescription; }
                        }
                    }
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
