using Shared.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.UI.Repositories.Interfaces
{
    public interface IPostalObjectRepository
    {
        public Task<PostalObjectViewModel> GetPostalObjectInfo(DataTable ServiceCompanyList, string PostObjBarCode);
    }
}
