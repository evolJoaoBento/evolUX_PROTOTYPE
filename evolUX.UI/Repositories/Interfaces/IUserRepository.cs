using evolUX.UI.Areas.Core.Models;
using Flurl.Http;

namespace evolUX.UI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task ChangeCulture(int userID, string culture);
    }
}
