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
using evolUX.API.Areas.Finishing.Models;
using Newtonsoft.Json;
using evolUX.UI.Areas.Core.Models;
using ErrorResult = evolUX.API.Areas.Core.ViewModels.ErrorResult;
using ErrorViewModel = evolUX.API.Areas.Core.ViewModels.ErrorViewModel;

namespace evolUX.UI.Areas.EvolDP.Controllers
{
    [Area("Finishing")]
    public class PrintController : Controller
    {
        private readonly IPrintService _printService;
        public PrintController(IPrintService printService)
        {
            _printService = printService;
        }

        public async Task<IActionResult> Printing(int RunID, int FileID, string FilePath, string FileName, string ShortFileName, string FilePrinterSpecs)
        {
            string profileList = HttpContext.Session.GetString("evolDP/Profiles");
            string filesSpecs = FilePrinterSpecs;
            bool ignoreProfiles = false;
            try
            {
                ResoursesViewModel result = await _printService.GetPrinters(profileList, filesSpecs, ignoreProfiles);
                result.RunID = RunID;
                result.FileID = FileID;
                result.FilePath = FilePath;
                result.FileName = FileName;
                result.ShortFileName = ShortFileName;
                result.FilePrinterSpecs = FilePrinterSpecs;
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

        public async Task<IActionResult> Print()
        {
            try
            {
                return View();
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
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.errorResult = await ex.response.GetJsonAsync<ErrorResult>();
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
