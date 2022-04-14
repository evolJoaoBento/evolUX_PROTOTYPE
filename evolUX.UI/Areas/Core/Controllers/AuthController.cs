using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace evolUX.UI.Areas.Core.Controllers
{
    [Area("Core")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {

            var errorResult = TempData["resultError"]?.ToString();
            if (errorResult != null)
                TempData["resultError"] = JsonSerializer.Deserialize<ErrorResult>(errorResult);
            //ViewBag.ErrorResult = errorResult;
            return View();
        }


        [Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LoginPost(string returnUrl)
        {
            var username = User.Identity?.Name?.Split("\\")[1];
            //chamada à api para ter o jwt e o user
            var response = await _authService.GetTokenAndUser(username);
            if (response.StatusCode == ((int)HttpStatusCode.NotFound))
            {
                var resultError = response.GetJsonAsync<ErrorResult>().Result;
                TempData["resultError"] = JsonSerializer.Serialize(resultError);
                return RedirectToAction("Index", "Auth");
            }
            var result = response.GetJsonAsync<AuthenticateResponse>().Result;
            Response.Cookies.Append("X-Access-Token", result.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.Username),
            };
            claims.AddRange(result.Roles.Select(role => new Claim(ClaimTypes.Role, role.Description)));
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                            new ClaimsPrincipal(claimsIdentity),
                                            new AuthenticationProperties
                                            {
                                                IsPersistent = false
                                            });

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Auth");
            }

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Auth");
        }


        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }

    }
}
