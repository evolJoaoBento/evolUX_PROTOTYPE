using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace evolUX.API.Areas.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public ActionResult<string> GetWindowsUsername()
        {
            var identity = _httpContextAccessor.HttpContext?.User?.Identity;
            if (identity is WindowsIdentity windowsIdentity && windowsIdentity.AuthenticationType == "NTLM")
            {
                var username = windowsIdentity.Name;
                var message = "";
                return Ok(username);
            }

            return NotFound();
        }
    }
}
