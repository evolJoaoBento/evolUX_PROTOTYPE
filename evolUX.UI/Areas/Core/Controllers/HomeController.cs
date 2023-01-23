using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Areas.Core.Services.Interfaces;
using evolUX.UI.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using System.Diagnostics;
using Flurl.Http;
using System.Net;

namespace evolUX.UI.Areas.Core.Controllers
{
    [Area("Core")]
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Manager")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestID = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CultureManagement(string culture, string returnurl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });
            try
            {
                AuthenticateResponse userInfo = JsonConvert.DeserializeObject<AuthenticateResponse>(HttpContext.Session.GetString("UserInfo"));
                _userService.ChangeCulture(userInfo.Id, culture);
                userInfo.Language = culture;
                HttpContext.Session.SetString("UserInfo", JsonConvert.SerializeObject(userInfo));
                return LocalRedirect(returnurl);
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