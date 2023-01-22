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
using Shared.ViewModels.Areas.Core;

namespace evolUX.UI.Areas.EvolDP.Controllers
{
    [Area("Finishing")]
    public class ConcludedFullfillController : Controller
    {
        private readonly IConcludedFullfillService _concludedFullfillService;
        public ConcludedFullfillController(IConcludedFullfillService concludedFullfillService)
        {
            _concludedFullfillService = concludedFullfillService;
        }

        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistFullFill(string FileBarcode)
        {
            DataTable ServiceCompanyList = HttpContext.Session.Get<DataTable>("evolDP/ServiceCompanies");
            String user = HttpContext.Session.Get<AuthenticateResponse>("UserInfo").Username;

            var response = await _concludedFullfillService.RegistFullFill(FileBarcode, user, ServiceCompanyList);
            if (response.StatusCode == ((int)HttpStatusCode.NotFound))
            {
                var resultError = response.GetJsonAsync<ErrorResult>().Result;
            }
            if(response.StatusCode == ((int)HttpStatusCode.Unauthorized))
            {
                if (response.Headers.Contains("Token-Expired"))
                {
                    var header = response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    //var url = Url.RouteUrl("MyAreas", )
                    
                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl=returnUrl });
                }
                else
                {
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
            
            ResultsViewModel result = await response.GetJsonAsync<ResultsViewModel>();
            return View("MessageView", new MessageViewModel(result.Results.ErrorID.ToString(), "", result.Results.Error));
        }  

    }
}
