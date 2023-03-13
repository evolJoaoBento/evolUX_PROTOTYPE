using Shared.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IPostalObjectService
    {
        public Task<PostalObjectViewModel> GetPostalObjectInfo(DataTable ServiceCompanyList, string PostObjBarCode);
    }
}
