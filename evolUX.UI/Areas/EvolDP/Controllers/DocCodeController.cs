using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Areas.EvolDP.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;

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
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> DocCodeIndex()
        {
            
            var response = await _docCodeService.GetDocCode();
            //if (response.StatusCode == ((int)HttpStatusCode.NotFound))
            //{
            //    var resultError = response.GetJsonAsync<ErrorResult>().Result;
            //}
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
            var result = await response.GetJsonListAsync();
            var list = (dynamic)result.ToList();
            return View(list);
        }


    }
}
