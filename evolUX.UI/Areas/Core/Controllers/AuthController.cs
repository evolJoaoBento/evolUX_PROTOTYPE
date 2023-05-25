using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Areas.Core.Services.Interfaces;
using evolUX.UI.Exceptions;
using evolUX.UI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Areas.Core;
using System.Data;
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
        public async Task<IActionResult> Index(string returnUrl)
        {
            //use claims to check user and load session variables

            if (User.Identity.IsAuthenticated)
            {
                if (HttpContext.Session.Keys.Count() == 0)
                {
                    int status = await HiddenRefresh();
                    if (status != 200)
                    {
                        return await Logout();
                    }
                }
            }
            var errorResult = TempData["resultError"]?.ToString();
            if (errorResult != null)
                TempData["resultError"] = JsonSerializer.Deserialize<ErrorResult>(errorResult);
            ViewData["returnUrl"] = returnUrl;
            return View();
        }


        [Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LoginWindowsAuthentication(string returnUrl)
        {
            try
            {
                var username = User.Identity?.Name;
                //chamada à api para ter o jwt e o user
                var response = await _authService.GetTokenAndUser(username);
                //var header = response.Headers.FirstOrDefault(h => h.Name == "").Value;
                if (response.StatusCode == ((int)HttpStatusCode.NotFound))
                {
                    var resultError = response.GetJsonAsync<ErrorResult>().Result;
                    TempData["resultError"] = JsonSerializer.Serialize(resultError);
                    return RedirectToAction("Index", "Auth");
                }
                var result = response.GetJsonAsync<AuthenticateResponse>().Result;
                await SetSessionVariables(result);

                SetJWTCookie(result.AccessToken);
                SetRTCookie(result.RefreshToken);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.Username),
                    new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
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
            catch (ErrorViewModelException ex)
            {
                TempData["errorMessage"] = ex.ViewModel?.ErrorResult?.Message;
                return RedirectToAction("Index", "Auth");
            }
        }

        //[AllowAnonymous]
        //public IActionResult LoginCredentialsIndex()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        public async Task<IActionResult> LoginCredentials(UserLogin model, string returnUrl = null)
        {
            try
            {
                var response = await _authService.LoginCredentials(model.Username, model.Password);
                if (response.StatusCode == ((int)HttpStatusCode.NotFound))
                {
                    var resultError = response.GetJsonAsync<ErrorResult>().Result;
                    TempData["resultError"] = JsonSerializer.Serialize(resultError);
                    return RedirectToAction("Index", "Auth");
                }
                var result = response.GetJsonAsync<AuthenticateResponse>().Result;

                await SetSessionVariables(result);

                SetJWTCookie(result.AccessToken);
                SetRTCookie(result.RefreshToken);

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, result.Username), new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()) };
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
            catch (ErrorViewModelException ex)
            {
                TempData["errorMessage"] = ex.ViewModel?.ErrorResult?.Message;
                return RedirectToAction("Index", "Auth");
            }
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            Response.Cookies.Delete("X-Access-Token");
            Response.Cookies.Delete("X-Refresh-Token");
            return RedirectToAction("Index", "Auth");
        }

        public async Task<IActionResult> Refresh(string returnUrl)
        {
            try
            {
                //also use claims to refresh session variables
                var token = Request.Cookies["X-Access-Token"];
                var refresh = Request.Cookies["X-Refresh-Token"];
                var response = await _authService.GetRefreshToken(token, refresh);
                if (response.StatusCode != Ok().StatusCode)
                    return null;
                var result = await response.GetJsonAsync();
                SetJWTCookie(((dynamic)result).accessToken);
                SetRTCookie(((dynamic)result).refreshToken);
                AuthenticateResponse user = new AuthenticateResponse();
                user.Id = (int)((dynamic)result).userModel.userID;
                user.Username = ((dynamic)result).userModel.userName;
                user.Language = ((dynamic)result).userModel.language;
                await SetSessionVariables(user);
                return Redirect(returnUrl);
            }
            catch (ErrorViewModelException ex)
            {
                TempData["errorMessage"] = ex.ViewModel?.ErrorResult?.Message;
                return RedirectToAction("Index", "Auth");
            }

        }
        public async Task<int> HiddenRefresh()
        {
            try
            {
                //also use claims to refresh session variables
                var token = Request.Cookies["X-Access-Token"];
                var refresh = Request.Cookies["X-Refresh-Token"];
                var response = await _authService.GetRefreshToken(token, refresh);
                if (response.StatusCode != Ok().StatusCode)
                    return response.StatusCode;
                var result = await response.GetJsonAsync();
                SetJWTCookie(((dynamic)result).accessToken);
                SetRTCookie(((dynamic)result).refreshToken);
                AuthenticateResponse user = new AuthenticateResponse();
                user.Id = (int)((dynamic)result).userModel.userID;
                user.Username = ((dynamic)result).userModel.userName;
                user.Language = ((dynamic)result).userModel.language;
                await SetSessionVariables(user);
                return response.StatusCode;
            }
            catch (ErrorViewModelException ex)
            {
                return ex.ViewModel.ErrorResult.Code;
            }
        }
        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }

        private void SetRTCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7),
            };
            Response.Cookies.Append("X-Refresh-Token", refreshToken, cookieOptions);
        }

        private void SetJWTCookie(string JWToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)

            };
            Response.Cookies.Append("X-Access-Token", JWToken, cookieOptions);
        }
        private async Task SetSessionVariables(AuthenticateResponse user)
        {
            HttpContext.Session.SetString("HasSession", "true");
            if (!string.IsNullOrEmpty(user.Language))
            {
                Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(user.Language)),
                    new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });
            }

            Dictionary<string, string> dictionary = await _authService.GetSessionVariables(user.Id);
            if (dictionary.IsNullOrEmpty())
            {
                //log error
            }
            else
            {
                foreach (string key in dictionary.Keys)
                {
                    HttpContext.Session.SetString(key, dictionary[key]);
                }
            }
            HttpContext.Session.Set<AuthenticateResponse>("UserInfo", user);
        }
    }
}

//using evolUX.UI.Areas.Core.Models;
//using evolUX.UI.Areas.Core.Services.Interfaces;
//using evolUX.UI.Exceptions;
//using evolUX.UI.Extensions;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.Negotiate;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.DataProtection.KeyManagement;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Localization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using Shared.Models.Areas.Core;
//using System.Data;
//using System.Net;
//using System.Security.Claims;
//using System.Text.Json;

//namespace evolUX.UI.Areas.Core.Controllers
//{
//    [Area("Core")]
//    public class AuthController : Controller
//    {
//        private readonly IAuthService _authService;

//        public AuthController(IAuthService authService)
//        {
//            _authService = authService;
//        }
//        [AllowAnonymous]
//        public async Task<IActionResult> Index(string returnUrl)
//        {
//            //use claims to check user and load session variables

//            if (User.Identity.IsAuthenticated)
//            {
//                if (HttpContext.Session.Keys.Count() == 0)
//                {
//                    int status = await HiddenRefresh();
//                    if (status != 200)
//                    {
//                        return await Logout();
//                    }
//                }
//            }
//            var errorResult = TempData["resultError"]?.ToString();
//            if (errorResult != null)
//                TempData["resultError"] = JsonSerializer.Deserialize<ErrorResult>(errorResult);
//            ViewData["returnUrl"] = returnUrl;
//            return View();
//        }


//        [Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
//        public async Task<IActionResult> LoginWindowsAuthentication(string returnUrl)
//        {
//            try 
//            { 
//                var username = User.Identity?.Name;
//                //chamada à api para ter o jwt e o user
//                var response = await _authService.GetTokenAndUser(username);
//                //var header = response.Headers.FirstOrDefault(h => h.Name == "").Value;
//                if (response.StatusCode == ((int)HttpStatusCode.NotFound))
//                {
//                    var resultError = response.GetJsonAsync<ErrorResult>().Result;
//                    TempData["resultError"] = JsonSerializer.Serialize(resultError);
//                    return RedirectToAction("Index", "Auth");
//                }
//                var result = response.GetJsonAsync<AuthenticateResponse>().Result;
//                await SetSessionVariables(result);

//                SetJWTCookie(result.AccessToken);
//                SetRTCookie(result.RefreshToken);
//                var claims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.Name, result.Username),
//                    new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
//                };
//                claims.AddRange(result.Roles.Select(role => new Claim(ClaimTypes.Role, role.Description)));
//                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
//                                                new ClaimsPrincipal(claimsIdentity),
//                                                new AuthenticationProperties
//                                                {
//                                                    IsPersistent = false
//                                                });

//                if (!string.IsNullOrEmpty(returnUrl))
//                {
//                    return Redirect(returnUrl);
//                }
//                else
//                {
//                    return RedirectToAction("Index", "Auth");
//                }
//            }
//            catch (ErrorViewModelException ex)
//            {
//                TempData["errorMessage"] = ex.ViewModel?.ErrorResult?.Message;
//                return RedirectToAction("Index", "Auth");
//            }
//        }


//        //[AllowAnonymous]
//        //public IActionResult LoginCredentialsIndex()
//        //{
//        //    return View();
//        //}

//        [Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]

//        public async Task<IActionResult> LoginADAuthentication(string returnUrl)
//        {
//            try
//            {
//                var username = HttpContext.User.Identity.Name;
//                //chamada à api para ter o jwt e o user
//                var response = await _authService.GetTokenAndUser(username);
//                //var header = response.Headers.FirstOrDefault(h => h.Name == "").Value;
//                if (response.StatusCode == ((int)HttpStatusCode.NotFound))
//                {
//                    var resultError = response.GetJsonAsync<ErrorResult>().Result;
//                    TempData["resultError"] = JsonSerializer.Serialize(resultError);
//                    return RedirectToAction("Index", "Auth");
//                }
//                var result = response.GetJsonAsync<AuthenticateResponse>().Result;
//                await SetSessionVariables(result);

//                SetJWTCookie(result.AccessToken);
//                SetRTCookie(result.RefreshToken);
//                var claims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.Name, result.Username),
//                    new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
//                };
//                claims.AddRange(result.Roles.Select(role => new Claim(ClaimTypes.Role, role.Description)));
//                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
//                                                new ClaimsPrincipal(claimsIdentity),
//                                                new AuthenticationProperties
//                                                {
//                                                    IsPersistent = false
//                                                });

//                if (!string.IsNullOrEmpty(returnUrl))
//                {
//                    return Redirect(returnUrl);
//                }
//                else
//                {
//                    return RedirectToAction("Index", "Auth");
//                }
//            }
//            catch (ErrorViewModelException ex)
//            {
//                TempData["errorMessage"] = ex.ViewModel?.ErrorResult?.Message;
//                return RedirectToAction("Index", "Auth");
//            }
//        }
//        public async Task<IActionResult> LoginCredentials(UserLogin model, string returnUrl = null)
//        {
//            try
//            {
//                var response = await _authService.LoginCredentials(model.Username, model.Password);
//                if (response.StatusCode == ((int)HttpStatusCode.NotFound))
//                {
//                    var resultError = response.GetJsonAsync<ErrorResult>().Result;
//                    TempData["resultError"] = JsonSerializer.Serialize(resultError);
//                    return RedirectToAction("Index", "Auth");
//                }
//                var result = response.GetJsonAsync<AuthenticateResponse>().Result;

//                await SetSessionVariables(result);

//                SetJWTCookie(result.AccessToken);
//                SetRTCookie(result.RefreshToken);

//                var claims = new List<Claim>{new Claim(ClaimTypes.Name, result.Username), new Claim(ClaimTypes.NameIdentifier, result.Id.ToString())};
//                claims.AddRange(result.Roles.Select(role => new Claim(ClaimTypes.Role, role.Description)));
//                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
//                                                new ClaimsPrincipal(claimsIdentity),
//                                                new AuthenticationProperties
//                                                {
//                                                    IsPersistent = false
//                                                });


//                if (!string.IsNullOrEmpty(returnUrl))
//                {
//                    return Redirect(returnUrl);
//                }
//                else
//                {
//                    return RedirectToAction("Index", "Auth");
//                }
//            }
//            catch (ErrorViewModelException ex)
//            {
//                TempData["errorMessage"] = ex.ViewModel?.ErrorResult?.Message;
//                return RedirectToAction("Index", "Auth");
//            }
//        }


//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync();
//            Response.Cookies.Delete("X-Access-Token");
//            Response.Cookies.Delete("X-Refresh-Token");
//            return RedirectToAction("Index", "Auth");
//        }

//        public async Task<IActionResult> Refresh(string returnUrl)
//        {
//            try
//            {
//                //also use claims to refresh session variables
//                var token = Request.Cookies["X-Access-Token"];
//                var refresh = Request.Cookies["X-Refresh-Token"];
//                var response = await _authService.GetRefreshToken(token, refresh);
//                if (response.StatusCode != Ok().StatusCode)
//                    return null;
//                var result = await response.GetJsonAsync();
//                SetJWTCookie(((dynamic)result).accessToken);
//                SetRTCookie(((dynamic)result).refreshToken);
//                AuthenticateResponse user = new AuthenticateResponse();
//                user.Id = (int)((dynamic)result).userModel.userID;
//                user.Username = ((dynamic)result).userModel.userName;
//                user.Language = ((dynamic)result).userModel.language;
//                await SetSessionVariables(user);
//                return Redirect(returnUrl);
//            }
//            catch (ErrorViewModelException ex)
//            {
//                TempData["errorMessage"] = ex.ViewModel?.ErrorResult?.Message;
//                return RedirectToAction("Index", "Auth");
//            }

//        }
//        public async Task<int> HiddenRefresh()
//        {
//            try
//            {
//                //also use claims to refresh session variables
//                var token = Request.Cookies["X-Access-Token"];
//                var refresh = Request.Cookies["X-Refresh-Token"];
//                var response = await _authService.GetRefreshToken(token, refresh);
//                if (response.StatusCode != Ok().StatusCode)
//                    return response.StatusCode;
//                var result = await response.GetJsonAsync();
//                SetJWTCookie(((dynamic)result).accessToken);
//                SetRTCookie(((dynamic)result).refreshToken);
//                AuthenticateResponse user = new AuthenticateResponse();
//                user.Id = (int)((dynamic)result).userModel.userID;
//                user.Username = ((dynamic)result).userModel.userName;
//                user.Language = ((dynamic)result).userModel.language;
//                await SetSessionVariables(user);
//                return response.StatusCode;
//            }
//            catch (ErrorViewModelException ex)
//            {
//                return ex.ViewModel.ErrorResult.Code;
//            }
//        }
//        public IActionResult AccessDenied(string returnUrl)
//        {
//            return View();
//        }

//        private void SetRTCookie(string refreshToken)
//        {
//            var cookieOptions = new CookieOptions
//            {
//                HttpOnly = true,
//                SameSite = SameSiteMode.Strict,
//                Expires = DateTime.UtcNow.AddDays(7),
//            };
//            Response.Cookies.Append("X-Refresh-Token", refreshToken, cookieOptions);
//        }

//        private void SetJWTCookie(string JWToken)
//        {
//            var cookieOptions = new CookieOptions
//            {
//                HttpOnly = true,
//                SameSite = SameSiteMode.Strict,
//                Expires = DateTime.UtcNow.AddDays(7)

//            };
//            Response.Cookies.Append("X-Access-Token", JWToken, cookieOptions);
//        }
//        private async Task SetSessionVariables(AuthenticateResponse user)
//        {
//            if (!string.IsNullOrEmpty(user.Language))
//            {
//                Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(user.Language)),
//                    new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });
//            }

//            Dictionary<string, string> dictionary = await _authService.GetSessionVariables(user.Id);
//            if (dictionary.IsNullOrEmpty())
//            {
//                //log error
//            }
//            else
//            {
//                foreach (string key in dictionary.Keys)
//                {
//                    HttpContext.Session.SetString(key, dictionary[key]);
//                }
//            }
//            HttpContext.Session.Set<AuthenticateResponse>("UserInfo", user);
//        }
//    }
//}
