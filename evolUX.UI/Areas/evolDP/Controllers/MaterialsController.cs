using evolUX.UI.Areas.evolDP.Services.Interfaces;
using evolUX.UI.Exceptions;
using evolUX_dev.Areas.evolDP.Models;
using Flurl.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.evolDP;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace evolUX.UI.Areas.evolDP.Controllers
{
    [Area("evolDP")]
    public class MaterialsController : Controller
    {
        private readonly IMaterialsService _materialsService;
        public MaterialsController(IMaterialsService materialsService)
        {
            _materialsService = materialsService;
        }
        public async Task<IActionResult> Management()
        {
            try
            {
                MaterialTypeViewModel result = new MaterialTypeViewModel();
                result.MaterialTypeList = await _materialsService.GetMaterialTypes(true,"");
                if (result != null && result.MaterialTypeList != null && result.MaterialTypeList.Count() > 0)
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
        
        public async Task<IActionResult> MaterialGoupsList(string materialTypeJSON)
        {
            try
            {
                MaterialType materialType = JsonConvert.DeserializeObject<MaterialType>(materialTypeJSON);
                string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
                MaterialViewModel result = new MaterialViewModel();
                result.MaterialTypeCode = materialType.MaterialTypeCode;
                result.MaterialList = await _materialsService.GetMaterialGroups(materialType.MaterialTypeCode, serviceCompanyList);
                result.FullfillMaterialCodes = await _materialsService.GetFulfillMaterialCodes();
                result.ServiceCompanies = JsonConvert.DeserializeObject<List<Company>>(serviceCompanyList);
                string CompaniesList = HttpContext.Session.GetString("evolDP/Companies");
                result.Companies = JsonConvert.DeserializeObject<List<Company>>(CompaniesList);
                ((List<Company>)result.Companies).AddRange(result.ServiceCompanies);
                if (result.MaterialList != null && (result.MaterialTypeCode.ToUpper() == "STATION" || result.MaterialList.Count() > 0))
                {
                    result.MaterialTypeList = await _materialsService.GetMaterialTypes(materialType.MaterialTypeCode);
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    return View(result);
                }
                else
                {
                    MaterialElement materialGroup = new MaterialElement();
                    materialGroup.GroupID = 0;
                    return RedirectToAction("MaterialList", "Materials", new { Area = "evolDP", materialGroupJSON = JsonConvert.SerializeObject(materialGroup), materialTypeCode = materialType.MaterialTypeCode });
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
        
        public async Task<IActionResult> ChangeMaterialGroup(IFormCollection form, string materialTypeCode)
        {
            try
            {
                MaterialViewModel result = new MaterialViewModel();
                result.MaterialTypeCode = materialTypeCode;
                MaterialElement group = new MaterialElement();

                int value = 0;
                double dValue = 0;
                string str;
                group.GroupID = 0;
                str = form["GroupID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    group.GroupID = value;

                group.GroupCode = form["GroupCode"].ToString();
                group.GroupDescription = form["GroupDescription"].ToString();
                group.MaterialTypeID = int.Parse(form["MaterialTypeID"].ToString());

                group.MaterialWeight = 0;
                str = form["MaterialWeight"].ToString();
                if (!string.IsNullOrEmpty(str) && double.TryParse(str, out dValue))
                {
                    if (dValue.ToString("F2") != str)
                    {
                        if (double.TryParse(str.Replace(".", ","), out dValue))
                            group.MaterialWeight = dValue;
                    }
                    else
                        group.MaterialWeight = dValue;
                }
                group.FullFillSheets = 1;
                str = form["FullFillSheets"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    group.FullFillSheets = value;

                group.FullFillMaterialCode = form["FullFillMaterialCode"].ToString();

                group.ExpeditionMinWeight = 0;
                str = form["ExpeditionMinWeight"].ToString();
                if (!string.IsNullOrEmpty(str) && double.TryParse(str, out dValue))
                {
                    if (dValue.ToString("F2") != str)
                    {
                        if (double.TryParse(str.Replace(".", ","), out dValue))
                            group.ExpeditionMinWeight = dValue;
                    }
                    else
                        group.ExpeditionMinWeight = dValue;
                }
                group.CostList = new List<MaterialCostElement>();
                MaterialCostElement costElement = new MaterialCostElement();
                costElement.ProviderCompanyID = 0;
                costElement.ServiceCompanyID = 0;
                costElement.CostDate = 0;
                costElement.MaterialCost = 0;
                costElement.MaterialBinPosition = 0;
                str = form["ProviderCompanyID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    costElement.ProviderCompanyID = value;
                //str = form["ServiceCompanyID"].ToString();
                //if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                //    costElement.ServiceCompanyID = value;
                str = form["CostDate"].ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    DateTime sDate;
                    if (DateTime.TryParse(str, out sDate))
                        str = sDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out value))
                        costElement.CostDate = value;
                    ((List<MaterialCostElement>)group.CostList).Add(costElement);
                }
                str = form["MaterialBinPosition"].ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    string[] mPos = str.Split(new char[] { ',' });
                    for(int i = 0; i < mPos.Length; i++) { costElement.MaterialBinPosition += Int32.Parse(mPos[i]); }
                }

                string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");

                result.Group = await _materialsService.SetMaterialGroup(group, serviceCompanyList);
                result.MaterialList = await _materialsService.GetMaterials(result.Group.GroupID, materialTypeCode, serviceCompanyList);
                result.MaterialTypeList = await _materialsService.GetMaterialTypes(materialTypeCode);
                result.FullfillMaterialCodes = await _materialsService.GetFulfillMaterialCodes();
                result.ServiceCompanies = JsonConvert.DeserializeObject<List<Company>>(serviceCompanyList);
                string CompaniesList = HttpContext.Session.GetString("evolDP/Companies");
                result.Companies = JsonConvert.DeserializeObject<List<Company>>(CompaniesList);
                ((List<Company>)result.Companies).AddRange(result.ServiceCompanies);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                return View("MaterialList",result);
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

        public async Task<IActionResult> MaterialList(string materialGroupJSON, string materialTypeCode)
        {
            try
            {
                string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
                MaterialViewModel result = new MaterialViewModel();
                result.MaterialTypeCode = materialTypeCode;
                result.Group = JsonConvert.DeserializeObject<MaterialElement>(materialGroupJSON);
                result.MaterialList = await _materialsService.GetMaterials(result.Group.GroupID, materialTypeCode, serviceCompanyList);
                result.MaterialTypeList = await _materialsService.GetMaterialTypes(materialTypeCode);
                result.FullfillMaterialCodes = await _materialsService.GetFulfillMaterialCodes();
                result.ServiceCompanies = JsonConvert.DeserializeObject<List<Company>>(serviceCompanyList);
                string CompaniesList = HttpContext.Session.GetString("evolDP/Companies");
                result.Companies = JsonConvert.DeserializeObject<List<Company>>(CompaniesList);
                ((List<Company>)result.Companies).AddRange(result.ServiceCompanies);
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

        public async Task<IActionResult> ChangeMaterial(IFormCollection form, string materialTypeCode)
        {
            try
            {
                MaterialViewModel result = new MaterialViewModel();
                result.MaterialTypeCode = materialTypeCode;
                MaterialElement material = new MaterialElement();

                int value = 0;
                double dValue = 0;
                string str;
                material.GroupID = 0;
                str = form["GroupID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    material.GroupID = value;

                material.MaterialID = 0;
                str = form["MaterialID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    material.MaterialID = value;

                material.MaterialRef = form["MaterialRef"].ToString();
                material.MaterialCode = form["MaterialCode"].ToString();
                if (string.IsNullOrEmpty(material.MaterialCode))
                        material.MaterialCode = material.MaterialRef;
                material.MaterialDescription = form["MaterialDescription"].ToString();
                material.MaterialTypeID = int.Parse(form["MaterialTypeID"].ToString());

                material.MaterialWeight = 0;
                str = form["MaterialWeight"].ToString();
                if (!string.IsNullOrEmpty(str) && double.TryParse(str, out dValue))
                {
                    if (dValue.ToString("F2") != str)
                    {
                        if (double.TryParse(str.Replace(".", ","), out dValue))
                            material.MaterialWeight = dValue;
                    }
                    else
                        material.MaterialWeight = dValue;
                }
                material.FullFillSheets = 1;
                str = form["FullFillSheets"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    material.FullFillSheets = value;

                material.FullFillMaterialCode = form["FullFillMaterialCode"].ToString();

                material.ExpeditionMinWeight = 0;
                str = form["ExpeditionMinWeight"].ToString();
                if (!string.IsNullOrEmpty(str) && double.TryParse(str, out dValue))
                {
                    if (dValue.ToString("F2") != str)
                    {
                        if (double.TryParse(str.Replace(".", ","), out dValue))
                            material.ExpeditionMinWeight = dValue;
                    }
                    else
                        material.ExpeditionMinWeight = dValue;
                }
                material.CostList = new List<MaterialCostElement>();
                MaterialCostElement costElement = new MaterialCostElement();
                costElement.ProviderCompanyID = 0;
                costElement.ServiceCompanyID = 0;
                costElement.CostDate = 0;
                costElement.MaterialCost = 0;
                costElement.MaterialBinPosition = 0;
                str = form["ProviderCompanyID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    costElement.ProviderCompanyID = value;
                //str = form["ServiceCompanyID"].ToString();
                //if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                //    costElement.ServiceCompanyID = value;
                str = form["CostDate"].ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    DateTime sDate;
                    if (DateTime.TryParse(str, out sDate))
                        str = sDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out value))
                        costElement.CostDate = value;
                    ((List<MaterialCostElement>)material.CostList).Add(costElement);
                }
                str = form["MaterialBinPosition"].ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    string[] mPos = str.Split(new char[] { ',' });
                    for (int i = 0; i < mPos.Length; i++) { costElement.MaterialBinPosition += Int32.Parse(mPos[i]); }
                }
                str = form["MaterialCost"].ToString();
                if (!string.IsNullOrEmpty(str) && double.TryParse(str, out dValue))
                {
                    if (dValue.ToString("F2") != str)
                    {
                        if (double.TryParse(str.Replace(".", ","), out dValue))
                            costElement.MaterialCost = dValue;
                    }
                    else
                        costElement.MaterialCost = dValue;
                }
                str = form["ServiceCompanyID"].ToString();
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out value))
                    costElement.ServiceCompanyID = value;
                material.CostList.Append(costElement);
                string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");

                result.Group = await _materialsService.SetMaterial(material, materialTypeCode, serviceCompanyList);
                result.MaterialList = await _materialsService.GetMaterials(result.Group.GroupID, materialTypeCode, serviceCompanyList);
                result.MaterialTypeList = await _materialsService.GetMaterialTypes(materialTypeCode);
                result.FullfillMaterialCodes = await _materialsService.GetFulfillMaterialCodes();
                result.ServiceCompanies = JsonConvert.DeserializeObject<List<Company>>(serviceCompanyList);
                string CompaniesList = HttpContext.Session.GetString("evolDP/Companies");
                result.Companies = JsonConvert.DeserializeObject<List<Company>>(CompaniesList);
                ((List<Company>)result.Companies).AddRange(result.ServiceCompanies);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                return View("MaterialList", result);
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

        public async Task<IActionResult> MaterialCost(string materialGroupJSON, string materialTypeCode)
        {
            try
            {
                string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
                MaterialCostViewModel result = new MaterialCostViewModel();
                result.MaterialTypeCode = materialTypeCode;
                result.Material = JsonConvert.DeserializeObject<MaterialElement>(materialGroupJSON);
                result.Material.CostList = await _materialsService.GetMaterialCost(result.Material.MaterialID, serviceCompanyList);
                result.Restrictions = await _materialsService.GetServiceCompanyRestrictions(result.Material.MaterialTypeID);
                result.FullfillMaterialCodes = await _materialsService.GetFulfillMaterialCodes();
                result.ServiceCompanies = JsonConvert.DeserializeObject<List<Company>>(serviceCompanyList);
                string CompaniesList = HttpContext.Session.GetString("evolDP/Companies");
                result.Companies = JsonConvert.DeserializeObject<List<Company>>(CompaniesList);
                ((List<Company>)result.Companies).AddRange(result.ServiceCompanies);
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
