using Shared.ViewModels.General;
using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Data;
using evolUX.UI.Extensions;
using Shared.Models.Areas.Core;
using evolUX.UI.Areas.Finishing.Services;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Newtonsoft.Json;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.Finishing;
using Shared.Exceptions;
using Shared.Models.General;

namespace evolUX.UI.Areas.EvolDP.Controllers
{
    [Area("Finishing")]
    public class PostalObjectController : Controller
    {
        private readonly IPostalObjectService _postalObjectService;
        public PostalObjectController(IPostalObjectService postalObjectService)
        {
            _postalObjectService = postalObjectService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetPostalObjectInfo(string PostObjBarcode)
        {
            string ServiceCompanyList = HttpContext.Session.GetString("evolDP/ServiceCompanies");
            try
            {
                PostalObjectViewModel result = await _postalObjectService.GetPostalObjectInfo(ServiceCompanyList, PostObjBarcode);
                return PartialView(result);
            }
            catch (ControledErrorException ex)
            {
                return PartialView("MessageView", ex.ControledMessage);
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
