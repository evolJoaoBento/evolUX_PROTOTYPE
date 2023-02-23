using evolUX.UI.Areas.EvolDP.Services.Interfaces;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.evolDP;
using Shared.ViewModels.General;
using System.Data;
using System.Globalization;
using System.Text;

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
                GetExceptionConfigs();
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

        public async Task<IActionResult> GetDocCodeConfig(string doccodeJson)
        {
            try
            {
                DocCode docCode = JsonConvert.DeserializeObject<DocCode>(doccodeJson);
                DocCodeConfigViewModel result = await _docCodeService.GetDocCodeConfig(docCode.DocCodeID);
                if (result != null)
                {
                    result.DocCode.DocLayout = docCode.DocLayout;
                    result.DocCode.DocType = docCode.DocType;
                    result.DocCode.PrintMatchCode = docCode.PrintMatchCode;
                    result.DocCode.DocDescription = docCode.DocDescription;
                    result.DocCode.ExceptionLevel1 = docCode.ExceptionLevel1;
                    result.DocCode.ExceptionLevel2 = docCode.ExceptionLevel2;
                    result.DocCode.ExceptionLevel3 = docCode.ExceptionLevel3;

                    result.SuportTypeList = GetConfigs();
                }
                return View("DocCodeConfigList", result);
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

        public async Task<IActionResult> AddDocCode(string doccodeJson, string source)
        {
            try
            {
                if (!string.IsNullOrEmpty(source))
                { TempData["Source"] = source; }

                DocCode? docCode = null;
                if (!string.IsNullOrEmpty(doccodeJson))
                    docCode = JsonConvert.DeserializeObject<DocCode>(doccodeJson);
                DocCodeConfigOptionsViewModel result = await _docCodeService.GetDocCodeConfigOptions(docCode);
                if (result != null)
                {
                    string ExpCompanyList = HttpContext.Session.GetString("evolDP/ExpeditionCompanies");
                    if (!string.IsNullOrEmpty(ExpCompanyList))
                        result.ExpCompanies = JsonConvert.DeserializeObject<List<Company>>(ExpCompanyList);

                    result.SuportTypeList = GetConfigs();
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

        public async Task<IActionResult> RegistDocCodeConfig(IFormCollection form, string source)
        {
            try
            {
                DocCode docCode = new DocCode();
                int intValue = 0;
                string strValue = form["DocCodeID"].ToString();
                if (string.IsNullOrEmpty(strValue) || !Int32.TryParse(strValue, out intValue))
                {
                    docCode.DocLayout = form["DocLayout"].ToString();
                    docCode.DocType = form["DocType"].ToString();

                    strValue = form["Exceptionslevel1ID"].ToString();
                    if (!string.IsNullOrEmpty(strValue) && Int32.TryParse(strValue, out intValue))
                    {
                        docCode.ExceptionLevel1 = new ExceptionLevel();
                        docCode.ExceptionLevel1.ExceptionLevelID = intValue;
                    }

                    strValue = form["Exceptionslevel2ID"].ToString();
                    if (!string.IsNullOrEmpty(strValue) && Int32.TryParse(strValue, out intValue))
                    {
                        docCode.ExceptionLevel2 = new ExceptionLevel();
                        docCode.ExceptionLevel2.ExceptionLevelID = intValue;
                    }

                    strValue = form["Exceptionslevel3ID"].ToString();
                    if (!string.IsNullOrEmpty(strValue) && Int32.TryParse(strValue, out intValue))
                    {
                        docCode.ExceptionLevel3 = new ExceptionLevel();
                        docCode.ExceptionLevel3.ExceptionLevelID = intValue;
                    }
                }
                else
                    docCode.DocCodeID = intValue;

                docCode.DocDescription = form["DocDescription"].ToString();
                docCode.PrintMatchCode = form["PrintMatchCode"].ToString();

                strValue = form["StartDate"].ToString();
                if (string.IsNullOrEmpty(strValue))
                {
                    DocCodeConfigViewModel result = await _docCodeService.ChangeDocCode(docCode);
                    if (result != null)
                    {
                        result.SuportTypeList = GetConfigs();
                        TempData["Source"] = source;
                        return View("DocCodeConfigList", result);
                    }
                    else
                    {
                        ErrorViewModel viewModel = new ErrorViewModel();
                        viewModel.ErrorResult = new ErrorResult();
                        viewModel.ErrorResult.Message = "EmptyResult";
                        return View("Error", viewModel);
                    }
                }
                else
                {
                    docCode.DocCodeConfigs.Add(new DocCodeConfig());
                    strValue = form["StartDate"].ToString();
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        DateTime sDate;
                        if (DateTime.TryParse(strValue, out sDate))
                            strValue = sDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(strValue) && Int32.TryParse(strValue, out intValue))
                        docCode.DocCodeConfigs[0].StartDate = intValue;
                    else
                        docCode.DocCodeConfigs[0].StartDate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));

                    strValue = form["EnvMediaID"].ToString();
                    if (!string.IsNullOrEmpty(strValue) && Int32.TryParse(strValue, out intValue))
                        docCode.DocCodeConfigs[0].EnvMediaID = intValue;

                    strValue = form["AggrCompatibility"].ToString();
                    if (!string.IsNullOrEmpty(strValue) && Int32.TryParse(strValue, out intValue))
                        docCode.DocCodeConfigs[0].AggrCompatibility = intValue;

                    strValue = form["Priority"].ToString();
                    if (!string.IsNullOrEmpty(strValue) && Int32.TryParse(strValue, out intValue))
                        docCode.DocCodeConfigs[0].Priority = intValue;

                    strValue = form["ProdMaxSheets"].ToString();
                    if (!string.IsNullOrEmpty(strValue) && Int32.TryParse(strValue, out intValue))
                        docCode.DocCodeConfigs[0].ProdMaxSheets = intValue;

                    docCode.DocCodeConfigs[0].ExpCode = form["ExpCode"].ToString();

                    strValue = form["ExpeditionType"].ToString();
                    if (!string.IsNullOrEmpty(strValue) && Int32.TryParse(strValue, out intValue))
                        docCode.DocCodeConfigs[0].ExpeditionType = intValue;

                    docCode.DocCodeConfigs[0].MaxProdDate = form["MaxProdDate"].ToString();
                    strValue = form["SuportType"].ToString();
                    if (!string.IsNullOrEmpty(strValue) && Int32.TryParse(strValue, out intValue))
                        docCode.DocCodeConfigs[0].SuportType = intValue;

                    docCode.DocCodeConfigs[0].ArchCaducityDate = form["ArchCaducityDate"].ToString();
                    docCode.DocCodeConfigs[0].CaducityDate = form["CaducityDate"].ToString();
                    DocCodeConfigViewModel result = await _docCodeService.RegistDocCodeConfig(docCode);
                    if (result != null)
                    {
                        result.SuportTypeList = GetConfigs();
                        TempData["Source"] = source;
                        return View("DocCodeConfig", result);
                    }
                    else
                    {
                        ErrorViewModel viewModel = new ErrorViewModel();
                        viewModel.ErrorResult = new ErrorResult();
                        viewModel.ErrorResult.Message = "EmptyResult";
                        return View("Error", viewModel);
                    }
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

        public async Task<IActionResult> DocCodeConfig(DocCode docCode, string doccodeJson)
        {
            try
            {
                if (!string.IsNullOrEmpty(doccodeJson))
                    docCode = JsonConvert.DeserializeObject<DocCode>(doccodeJson);

                DocCodeConfigViewModel result = new DocCodeConfigViewModel();
                result.DocCode = docCode;
                result.SuportTypeList = GetConfigs();
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

        public async Task<IActionResult> DeleteDocCodeConfig(int docCodeID, int startDate, string doccodeJson)
        {
            try
            {
                DocCode docCode = JsonConvert.DeserializeObject<DocCode>(doccodeJson);
                ResultsViewModel deleteResult = await _docCodeService.DeleteDocCodeConfig(docCodeID, startDate);
                if (deleteResult.Results.ErrorID == 0)
                {
                    DocCodeConfigViewModel result = await _docCodeService.GetDocCodeConfig(docCode.DocCodeID);
                    if (result != null)
                    {
                        docCode.DocCodeConfigs = result.DocCode.DocCodeConfigs;
                        result.DocCode = docCode;

                        result.SuportTypeList = GetConfigs();
                    }
                    return View("DocCodeConfigList", result);
                }
                else
                {
                    TempData["Message"] = deleteResult.Results.Error;
                    DocCodeConfigViewModel result = new DocCodeConfigViewModel();
                    result.DocCode = docCode;
                    string SuportTypeListJSON = HttpContext.Session.GetString("evolDP/SuportTypeList");
                    if (!string.IsNullOrEmpty(SuportTypeListJSON))
                        result.SuportTypeList = JsonConvert.DeserializeObject<GenericOptionList>(SuportTypeListJSON);
                    return View("DocCodeConfigList", result);
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

        public async Task<IActionResult> DocCodeData4Script(int docCodeID, int startDate)
        {
            try
            {
                List<string> script = await _docCodeService.DocCodeData4Script(docCodeID, startDate);
                if (script != null && script.Count() == 2 && !string.IsNullOrEmpty(script[0]))
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(script[0]);
                    Response.ContentType = "text/plain";
                    Response.Headers.Add("Content-Disposition", "attachment; filename=" + script[1]);

                    // End the response
                    return File(byteArray, "text/plain");

                }
                else return PartialView("Message", new MessageViewModel() { MessageDetail = "EmptyResult"});

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

        public async Task<IActionResult> DeleteDocCode(string doccodeJson)
        {
            try
            {
                DocCode docCode = JsonConvert.DeserializeObject<DocCode>(doccodeJson);

                ResultsViewModel deleteResult = await _docCodeService.DeleteDocCode(docCode);
                if (deleteResult.Results.ErrorID == 0)
                {
                    DocCodeViewModel result = await _docCodeService.GetDocCode(docCode.DocLayout, docCode.DocType);
                    if (result != null && result.DocCodeList != null && result.DocCodeList.Count() > 0)
                        return View("DocCode", result);
                    else
                        return View("Index");
                }
                else
                {
                    TempData["Message"] = deleteResult.Results.Error;
                    DocCodeConfigViewModel result = new DocCodeConfigViewModel();
                    result.DocCode = docCode;
                    result.SuportTypeList = GetConfigs();
                    return View("DocCodeConfigList", result);
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

        public async Task<IActionResult> AddAggCompatibility(string doccodeJson)
        {
            try
            {
                DocCode docCode = JsonConvert.DeserializeObject<DocCode>(doccodeJson);

  
                DocCodeCompatibilityViewModel viewmodel = await _docCodeService.GetCompatibility(docCode.DocCodeID);
                if (viewmodel != null)
                {
                    viewmodel.DocCode = docCode;
                    GetExceptionConfigs();
                    TempData["Message"] = "Success";
                    return View(viewmodel);

                }
                else
                {
                    ErrorViewModel viewModel = new ErrorViewModel();
                    viewModel.ErrorResult = new ErrorResult();
                    viewModel.ErrorResult.Message = "EmptyResult";
                    return View("Error", viewModel);
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
     
        public async Task<IActionResult> ChangeCompatibility(string doccodeJson, List<string> CheckedDocCodeList)
        {
            try
            {
                DocCode docCode = JsonConvert.DeserializeObject<DocCode>(doccodeJson);
 
                DocCodeCompatibilityViewModel viewmodel = await _docCodeService.ChangeCompatibility(docCode.DocCodeID, CheckedDocCodeList);
                if (viewmodel != null)
                {
                    viewmodel.DocCode = docCode;
                    return View("AddAggCompatibility", viewmodel);

                }
                else
                {
                    ErrorViewModel viewModel = new ErrorViewModel();
                    viewModel.ErrorResult = new ErrorResult();
                    viewModel.ErrorResult.Message = "EmptyResult";
                    return View("Error", viewModel);
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
 
        public async Task<IActionResult> ExceptionLevel(int Level)
        {
            try
            {
                ExceptionLevelViewModel result = await _docCodeService.GetExceptionLevel(Level);

                string evolDP_DescriptionJSON = HttpContext.Session.GetString("evolDP/evolDP_DESCRIPTION");
                string cultureCode = CultureInfo.CurrentCulture.Name;
                if (!string.IsNullOrEmpty(cultureCode))
                    cultureCode = cultureCode.Substring(0, 2);
                TempData["ExceptionLevelID"] = "";
                if (!string.IsNullOrEmpty(evolDP_DescriptionJSON))
                {
                    var evolDP_Desc = JsonConvert.DeserializeObject<List<dynamic>>(evolDP_DescriptionJSON);
                    if (evolDP_Desc != null)
                    {
                        bool b = false;
                        string except = string.Format("ExceptionLevel{0}ID", Level);
                        var val = evolDP_Desc.Find(x => x.FieldName == except + "_" + cultureCode);
                        if (val == null) { val = evolDP_Desc.Find(x => x.FieldName == except); }
                        if (val != null) { TempData["ExceptionLevelID"] = val.FieldDescription; }
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

        public async Task<IActionResult> ConfigExceptionLevel(IFormCollection form, string source, int Level)
        {
            try
            {
                int exceptionID = 0;
                string strValue = form["ExceptionLevelID"].ToString();
                string exceptionCode = form["ExceptionCode"].ToString();
                string exceptionDescription = form["ExceptionDescription"].ToString();
                if (!string.IsNullOrEmpty(strValue) && !Int32.TryParse(strValue, out exceptionID))
                    exceptionID = 0;

                ExceptionLevelViewModel result = await _docCodeService.SetExceptionLevel(Level, exceptionID, exceptionCode, exceptionDescription);

                string evolDP_DescriptionJSON = HttpContext.Session.GetString("evolDP/evolDP_DESCRIPTION");
                string cultureCode = CultureInfo.CurrentCulture.Name;
                if (!string.IsNullOrEmpty(cultureCode))
                    cultureCode = cultureCode.Substring(0, 2);
                TempData["ExceptionLevelID"] = "";
                if (!string.IsNullOrEmpty(evolDP_DescriptionJSON))
                {
                    var evolDP_Desc = JsonConvert.DeserializeObject<List<dynamic>>(evolDP_DescriptionJSON);
                    if (evolDP_Desc != null)
                    {
                        bool b = false;
                        string except = string.Format("ExceptionLevel{0}ID", Level);
                        var val = evolDP_Desc.Find(x => x.FieldName == except + "_" + cultureCode);
                        if (val == null) { val = evolDP_Desc.Find(x => x.FieldName == except); }
                        if (val != null) { TempData["ExceptionLevelID"] = val.FieldDescription; }
                    }
                }
                return View("ExceptionLevel",result);
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

        public async Task<IActionResult> DeleteExceptionLevel(int Level, int exceptionID)
        {
            try
            {
                ExceptionLevelViewModel result;
                if (exceptionID != 0)
                    result = await _docCodeService.DeleteExceptionLevel(Level, exceptionID);
                else
                    result = await _docCodeService.GetExceptionLevel(Level);

                string evolDP_DescriptionJSON = HttpContext.Session.GetString("evolDP/evolDP_DESCRIPTION");
                string cultureCode = CultureInfo.CurrentCulture.Name;
                if (!string.IsNullOrEmpty(cultureCode))
                    cultureCode = cultureCode.Substring(0, 2);
                TempData["ExceptionLevelID"] = "";
                if (!string.IsNullOrEmpty(evolDP_DescriptionJSON))
                {
                    var evolDP_Desc = JsonConvert.DeserializeObject<List<dynamic>>(evolDP_DescriptionJSON);
                    if (evolDP_Desc != null)
                    {
                        bool b = false;
                        string except = string.Format("ExceptionLevel{0}ID", Level);
                        var val = evolDP_Desc.Find(x => x.FieldName == except + "_" + cultureCode);
                        if (val == null) { val = evolDP_Desc.Find(x => x.FieldName == except); }
                        if (val != null) { TempData["ExceptionLevelID"] = val.FieldDescription; }
                    }
                }
                return View("ExceptionLevel", result);
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

        private void GetExceptionConfigs()
        {
            string evolDP_DescriptionJSON = HttpContext.Session.GetString("evolDP/evolDP_DESCRIPTION");
            string cultureCode = CultureInfo.CurrentCulture.Name;
            if (!string.IsNullOrEmpty(cultureCode))
                cultureCode = cultureCode.Substring(0, 2);
            TempData["ExceptionLevel1ID"] = "";
            TempData["ExceptionLevel2ID"] = "";
            TempData["ExceptionLevel3ID"] = "";
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
                }
            }
        }

        private GenericOptionList GetConfigs() 
        {
            GenericOptionList suportTypeList = null;
            string SuportTypeListJSON = HttpContext.Session.GetString("evolDP/SuportTypeList");
            if (!string.IsNullOrEmpty(SuportTypeListJSON))
                suportTypeList = JsonConvert.DeserializeObject<GenericOptionList>(SuportTypeListJSON);
            if (suportTypeList == null)
            {
                suportTypeList = new GenericOptionList();
                suportTypeList.List = new List<GenericOptionValue>();
                suportTypeList.OptionList = new List<GenericOptionValue>();
                suportTypeList.ValidList = new List<int>();
            }
            string evolDP_DescriptionJSON = HttpContext.Session.GetString("evolDP/evolDP_DESCRIPTION");
            string cultureCode = CultureInfo.CurrentCulture.Name;
            if (!string.IsNullOrEmpty(cultureCode))
                cultureCode = cultureCode.Substring(0, 2);
            TempData["ExceptionLevel1ID"] = "";
            TempData["ExceptionLevel2ID"] = "";
            TempData["ExceptionLevel3ID"] = "";
            foreach (GenericOptionValue option in suportTypeList.List)
                TempData[option.Code] = "";
            foreach (GenericOptionValue option in suportTypeList.OptionList)
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

                    foreach (GenericOptionValue option in suportTypeList.List)
                    {
                        val = evolDP_Desc.Find(x => x.FieldName == option.Code + "_" + cultureCode);
                        if (val == null) { val = evolDP_Desc.Find(x => x.FieldName == option.Code); }
                        if (val != null) { TempData[option.Code] = val.FieldDescription; }
                    }
                    foreach (GenericOptionValue option in suportTypeList.OptionList)
                    {
                        val = evolDP_Desc.Find(x => x.FieldName == option.GroupCode + "_" + cultureCode);
                        if (val == null) { val = evolDP_Desc.Find(x => x.FieldName == option.GroupCode); }
                        if (val != null) { TempData[option.GroupCode] = val.FieldDescription; }
                    }
                }
            }
            return suportTypeList;
        }
    }
}
