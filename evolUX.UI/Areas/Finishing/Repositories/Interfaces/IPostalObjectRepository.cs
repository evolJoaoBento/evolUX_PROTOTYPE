using Shared.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Repositories.Interfaces
{
    public interface IPostalObjectRepository
    {
        public Task<PostalObjectViewModel> GetPostalObjectInfo(string ServiceCompanyList, string PostObjBarCode);
    }
}
