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
                result.MaterialList = await _materialsService.GetMaterialGroups(materialType.MaterialTypeCode);
                if (result.MaterialList != null && result.MaterialList.Count() >= 0)
                {
                    result.MaterialTypeList = await _materialsService.GetMaterialTypes(false, materialType.MaterialTypeCode);
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

        public async Task<IActionResult> MaterialList(string materialGroupJSON, string materialTypeCode)
        {
            try
            {
                MaterialElement materialGroup = JsonConvert.DeserializeObject<MaterialElement>(materialGroupJSON);
                string serviceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
                MaterialViewModel result = new MaterialViewModel();
                result.MaterialTypeCode = materialTypeCode;
                result.MaterialList = await _materialsService.GetMaterials(materialGroup.GroupID, materialTypeCode);
                if (result.MaterialList != null && result.MaterialList.Count() > 0)
                {
                    result.MaterialTypeList = await _materialsService.GetMaterialTypes(false, materialTypeCode);
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
    }
}
