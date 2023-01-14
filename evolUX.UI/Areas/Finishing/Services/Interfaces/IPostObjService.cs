using Flurl.Http;
using Shared.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IPostObjService
    {
        public Task<PostObjViewModel> GetPostalObject(string ServiceCompanyList);
    }
}
