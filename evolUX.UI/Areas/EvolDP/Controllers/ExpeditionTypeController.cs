using evolUX.UI.Areas.evolDP.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace evolUX.UI.Areas.evolDP.Controllers
{
    [Area("EvolDP")]
    public class ExpeditionTypeController : Controller
    {
        private readonly IExpeditionTypeService _expeditionTypeService;

        public ExpeditionTypeController(IExpeditionTypeService expeditionTypeService)
        {
            _expeditionTypeService = expeditionTypeService;
        }
        public async Task<IActionResult> ExpeditionType()
        {
            var response = await _expeditionTypeService.GetExpeditionTypes();
            //if (response.StatusCode == ((int)HttpStatusCode.NotFound))
            //{
            //    var resultError = response.GetJsonAsync<ErrorResult>().Result;
            //}
            if (response.StatusCode == ((int)HttpStatusCode.Unauthorized))
            {
                if (response.Headers.Contains("Token-Expired"))
                {
                    var header = response.Headers.FirstOrDefault("Token-Expired");
                    var returnUrl = Request.Path.Value;
                    
                    return RedirectToAction("Refresh", "Auth", new { Area = "Core", returnUrl = returnUrl });
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    Response.Cookies.Delete("X-Access-Token");
                    Response.Cookies.Delete("X-Refresh-Token");
                    return RedirectToAction("Index", "Auth", new { Area = "Core" });
                }
            }
            var result = await response.GetJsonListAsync();
            var list = (dynamic)result.ToList();
            return View(list);
        }
    }
}
