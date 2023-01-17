using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IPostalObjectService
    {
        public Task<PostalObjectViewModel> GetPostalObjectInfo(string ServiceCompanyList, string PostObjBarCode);
    }
}
