using Shared.ViewModels.Areas.Finishing;

namespace evolUX.UI.Repositories.Interfaces
{
    public interface IPostalObjectRepository
    {
        public Task<PostalObjectViewModel> GetPostalObject(string ServiceCompanyList, string PostObjBarCode);
    }
}
