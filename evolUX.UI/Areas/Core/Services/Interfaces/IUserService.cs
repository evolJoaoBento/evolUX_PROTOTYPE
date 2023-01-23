using evolUX.UI.Areas.Core.Models;
using Flurl.Http;

namespace evolUX.UI.Areas.Core.Services.Interfaces
{
    public interface IUserService
    {
        public Task ChangeCulture(int userID, string culture);
    }
}
