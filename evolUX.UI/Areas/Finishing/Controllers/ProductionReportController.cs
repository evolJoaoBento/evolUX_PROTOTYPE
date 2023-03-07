using Shared.ViewModels.Areas.Finishing;
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
using Shared.Models.General;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using evolUX.API.Models;
using Shared.Models.Areas.evolDP;
using Shared.Models.Areas.Finishing;
using Microsoft.Extensions.Localization;
using evolUX_dev.Areas.evolDP.Models;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.Finishing.Controllers
{
    [Area("Finishing")]
    public class ProductionReportController : Controller
    {
        private readonly IProductionReportService _productionReportService;
        private readonly IStringLocalizer<ProductionReportController> _localizer;
        public ProductionReportController(IProductionReportService productionReportService, IStringLocalizer<ProductionReportController> localizer)
        {
            _productionReportService = productionReportService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
            try
            {
                if (string.IsNullOrEmpty(ServiceCompanyList))
                    return View(null);
                DataTable ServiceCompanies = JsonConvert.DeserializeObject<DataTable>(ServiceCompanyList);
                if (ServiceCompanies.Rows.Count > 1)
                {
                    ServiceCompaniesViewModel result = new ServiceCompaniesViewModel();
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    List<Company> sList = new List<Company>();
                    foreach(DataRow row in ServiceCompanies.Rows)
                    {
                        sList.Add(new Company
                        {
                            ID = Int32.Parse(row["ID"].ToString()),
                            CompanyCode = (string)row["CompanyCode"],
                            CompanyName = (string)row["CompanyName"]
                        });
                    }
                    result.ServiceCompanies = sList;
                    return View(result);
               }
                else
                {
                    string scValues = ServiceCompanies.Rows[0]["ID"].ToString() + "|" + ServiceCompanies.Rows[0]["CompanyCode"].ToString() + " | " + ServiceCompanies.Rows[0]["CompanyName"].ToString();
                    return RedirectToAction("ProductionRunReport", new { ServiceCompanyValues = scValues });
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

        public async Task<IActionResult> ProductionRunReport(string ServiceCompanyValues)
        {
            string[] serviceCompanyValue = ServiceCompanyValues.Split('|');
            int ServiceCompanyID = Convert.ToInt32(serviceCompanyValue[0]);
            string ServiceCompanyCode = serviceCompanyValue.Length > 1 ? serviceCompanyValue[1] : "";
            string ServiceCompanyName = serviceCompanyValue.Length > 2 ? serviceCompanyValue[2] : "";

            TempData["ServiceCompanyID"] = serviceCompanyValue[0];
            TempData["ServiceCompanyCode"] = ServiceCompanyCode;
            TempData["ServiceCompanyName"] = ServiceCompanyName;
            try
            {

                ProductionRunReportViewModel result = await _productionReportService.GetProductionRunReport(ServiceCompanyID);
                result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                DataTable ServiceCompanyDT = JsonConvert.DeserializeObject<DataTable>(HttpContext.Session.GetString("evolDP/ServiceCompanies"));
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

        public async Task<IActionResult> ProductionReport(string RunIDList, int ServiceCompanyID, string RunName)
        {
            try
            {
                List<int> runIDList = new List<int>();
                string[] runIDListStr = RunIDList.Split('|');
                if (string.IsNullOrEmpty(RunIDList) || runIDListStr.Length == 0)
                {
                    return PartialView("MessageView", new MessageViewModel(_localizer["Missing Runs"]));
                }
                foreach (string r in runIDListStr)
                    runIDList.Add(int.Parse(r));

                string profileList = HttpContext.Session.GetString("evolUX/Profiles");
                ProductionReportViewModel result = await _productionReportService.GetProductionReport(profileList, runIDList, ServiceCompanyID, false);

                if (result != null && result.ProductionReport != null && result.ProductionReport.Count() > 0)
                {
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    //TempData["ServiceCompanyCode"] = result.ProductionReport.First().ServiceCompanyCode;
                }
                ViewBag.RunName = RunName;
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

        public async Task<IActionResult> ProductionReportPrinter(string RunIDList, int ServiceCompanyID, string RunName)
        {
            try
            {
                List<int> runIDList = new List<int>();
                string[] runIDListStr = RunIDList.Split('|');
                if (string.IsNullOrEmpty(RunIDList) || runIDListStr.Length == 0)
                {
                    return PartialView("MessageView", new MessageViewModel(_localizer["Missing Runs"]));
                }
                foreach(string r in runIDListStr)
                    runIDList.Add(int.Parse(r));

                string profileList = HttpContext.Session.GetString("evolUX/Profiles");
                ProductionReportViewModel result = await _productionReportService.GetProductionReport(profileList, runIDList, ServiceCompanyID, true);
                IEnumerable<ProductionDetailInfo> filters = await _productionReportService.GetProductionReportFilters(profileList, runIDList, ServiceCompanyID, true);

                if (result != null)
                {
                    result.SetPermissions(HttpContext.Session.GetString("evolUX/Permissions"));
                    if (result.ProductionReport != null && result.ProductionReport.Count() > 0)
                    {
                        
                    }
                    if (result.Printers != null && result.Printers.Count() > 0)
                    {
                        List<PrinterInfo> printerInfos = result.Printers.ToList();
                        PrinterInfo p = new PrinterInfo();
                        p.PlexFeature = 3;
                        p.ColorFeature = 3;
                        p.ResValue = string.Empty;
                        p.Description = _localizer["SelectPrinter"];
                        printerInfos.Insert(0,p);
                        result.Printers = printerInfos;
                    }
                }
                result.filters = filters;
                ViewBag.RunName = RunName;
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
        public async Task<IActionResult> ProductionReportPrinterFilters(string RunIDList, int ServiceCompanyID, string RunName)
        {
            try
            {
                List<int> runIDList = new List<int>();
                string[] runIDListStr = RunIDList.Split('|');
                if (string.IsNullOrEmpty(RunIDList) || runIDListStr.Length == 0)
                {
                    return PartialView("MessageView", new MessageViewModel(_localizer["Missing Runs"]));
                }
                foreach (string r in runIDListStr)
                    runIDList.Add(int.Parse(r));

                string profileList = HttpContext.Session.GetString("evolUX/Profiles");
                IEnumerable<ProductionDetailInfo> result = await _productionReportService.GetProductionReportFilters(profileList, runIDList, ServiceCompanyID, true);
                
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
