using Shared.ViewModels.Areas.Finishing;
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
using evolUX.UI.Areas.Core.Models;
using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.General;
using System.Reflection;
using Shared.Models.Areas.Finishing;
using static Dapper.SqlMapper;
using System.Security.Claims;
using Shared.Models.General;
using Shared.ViewModels.Areas.Core;

namespace evolUX.UI.Areas.Finishing.Controllers
{
    [Area("Finishing")]
    public class PrintController : Controller
    {
        private readonly IPrintService _printService;
        public PrintController(IPrintService printService)
        {
            _printService = printService;
        }

        public async Task<IActionResult> Printing(string JsonSerializedProductionInfo, string FilePrinterSpecs, 
            string FileName)
        {
            TempData["JsonSerializedProductionInfo"] = JsonSerializedProductionInfo;
            
            ViewBag.FileName = FileName;

            string profileList = HttpContext.Session.GetString("evolUX/Profiles");
            string filesSpecs = FilePrinterSpecs;
            bool ignoreProfiles = false;
            try
            {

                ResoursesViewModel result = await _printService.GetPrinters(profileList, filesSpecs, ignoreProfiles);
                return View(result);
            }
            catch (ErrorViewModelException ex)
            {
                return View("Error", ex.ViewModel);
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

        public async Task<IActionResult> Print(string Printer)
        {
            string JsonSerializedProductionInfo = (string)TempData["JsonSerializedProductionInfo"];
            string ServiceCompanyCode = (string)TempData["ServiceCompanyCode"];
            ProductionInfo productionInfo = JsonConvert.DeserializeObject<ProductionInfo>(JsonSerializedProductionInfo);
            string username = User.Identity.Name;
            int userid = int.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                              .Select(c => c.Value)
                                              .SingleOrDefault());
            try
            {
                Result result = await _printService.Print(productionInfo.RunID, productionInfo.FileID, Printer, 
                    ServiceCompanyCode,
                            username, userid, productionInfo.FilePath, productionInfo.FileName, productionInfo.ShortFileName);
                return PartialView("MessageView", new MessageViewModel(result.ErrorID.ToString(), "", result.Error));
            }
            catch (ErrorViewModelException ex)
            {
                return PartialView("Error", ex.ViewModel);
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
