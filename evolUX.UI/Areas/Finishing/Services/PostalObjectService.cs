using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Repositories.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using System.Reflection.Metadata;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class PostalObjectService : IPostalObjectService
    {
        private readonly IPostalObjectRepository _postalObjectRepository;
        public PostalObjectService(IPostalObjectRepository postalObjectRepository)
        {
            _postalObjectRepository = postalObjectRepository;
        }
        public async Task<PostalObjectViewModel> GetPostalObject(string ServiceCompanyList, string PostObjBarCode)
        {
            var response = await _postalObjectRepository.GetPostalObject(ServiceCompanyList, PostObjBarCode);
            return response;
        }
    }
}
