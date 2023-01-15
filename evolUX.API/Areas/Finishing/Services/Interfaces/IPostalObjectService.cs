using Shared.ViewModels.Areas.Finishing;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IPostalObjectService
    {
        public Task<PostalObjectViewModel> GetPostalObject(string ServiceCompanyList, string PostObjBarCode);
    }
}
