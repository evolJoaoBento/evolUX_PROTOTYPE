using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using System.Data;
using evolUX.API.Areas.Core.Repositories.Interfaces;

namespace evolUX.API.Areas.Finishing.Services
{
    public class PostalObjectService : IPostalObjectService
    {
        private readonly IWrapperRepository _repository;
        public PostalObjectService(IWrapperRepository repository)
        {
            _repository = repository;
        }



        public async Task<PostalObjectViewModel> GetPostalObjectInfo(DataTable serviceCompanyList, string postObjBarcode)
        {
            PostalObjectViewModel viewmodel = new PostalObjectViewModel();
            viewmodel.PostalObject = (PostalObjectInfo)await _repository.PostalObject.GetPostalObjectInfo(serviceCompanyList, postObjBarcode);
            if (viewmodel.PostalObject == null || (viewmodel.PostalObject != null && viewmodel.PostalObject.Error.ToUpper() == "NOTSUCCESS"))
            {
                //TODO - Deu erro na execução deve ser tratado o erro
            }
            return viewmodel;
        }
    }
}
