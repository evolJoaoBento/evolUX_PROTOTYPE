using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Exceptions;
using evolUX.UI.Repositories.Interfaces;
using Flurl.Http;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.Finishing;
using System.Data;
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
        public async Task<PostalObjectViewModel> GetPostalObjectInfo(DataTable ServiceCompanyList, string PostObjBarCode)
        {
            try
            {
                var response = await _postalObjectRepository.GetPostalObjectInfo(ServiceCompanyList, PostObjBarCode);
                return response;
            }
            catch (FlurlHttpException ex)
            {
                // For error responses that take a known shape
                //TError e = ex.GetResponseJson<TError>();
                // For error responses that take an unknown shape
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = ex.StatusCode != null ? (int)ex.StatusCode : 0;
                viewModel.ErrorResult.Message = ex.Message;
                throw new ErrorViewModelException(viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult = await ex.response.GetJsonAsync<ErrorResult>();
                throw new ErrorViewModelException(viewModel);
            }
        }
    }
}
