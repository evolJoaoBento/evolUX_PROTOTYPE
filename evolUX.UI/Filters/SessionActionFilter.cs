using evolUX.UI.Areas.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Text.RegularExpressions;

namespace evolUX.UI.Filters
{
    public class SessionActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            if (controllerName.Equals("Auth"))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var context = filterContext.HttpContext;
            if (context.Session != null)
            {
                if (string.IsNullOrEmpty(context.Session.GetString("HasSession"))) // Session has expired
                {
                    var originalUrl = context.Request.Path + context.Request.QueryString;
                    filterContext.Result = new RedirectToActionResult("Index", "Auth", new { returnUrl = originalUrl });
                    return;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
