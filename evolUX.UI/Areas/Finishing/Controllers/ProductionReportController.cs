using evolUX.API.Areas.Finishing.ViewModels;
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

namespace evolUX.UI.Areas.EvolDP.Controllers
{
    [Area("Finishing")]
    public class ProductionReportController : Controller
    {
        private readonly IProductionReportService _productionReportService;
        public ProductionReportController(IProductionReportService productionReportService)
        {
            _productionReportService = productionReportService;
        }

        public async Task<IActionResult> ProductionRunReport()
        {
            string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
            try
            {
                ProductionRunReportViewModel result = await _productionReportService.GetProductionRunReport(ServiceCompanyList);
                return View(result);
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestId = ex.Source;
                viewModel.errorResult = new ErrorResult();
                viewModel.errorResult.Code = (int)ex.StatusCode;
                viewModel.errorResult.Message = ex.Message;
                return View("Error", viewModel);
            }
            catch(HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.errorResult = await ex.response.GetJsonAsync<ErrorResult>();
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
        public async Task<IActionResult> ProductionReport(int RunID, int ServiceCompanyID, string RunName)
        {
            try
            {
                ProductionReportViewModel result = await _productionReportService.GetProductionReport(RunID, ServiceCompanyID);
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
        [HttpPost]
        public ActionResult FilePrint(string model, int RunID, int FileID, string FilePath, string FileName, string ShortFileName, string FilePrinterSpecs)
        {
            return RedirectToAction("Printing", "Print", new { Area = "Finishing", RunID, FileID, FilePath, FileName, ShortFileName, FilePrinterSpecs });

        }


    }
}
