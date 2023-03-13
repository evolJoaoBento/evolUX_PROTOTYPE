using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Shared.Exceptions;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.General;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class RecoverService : IRecoverService
    {
        private readonly IRecoverRepository _recoverRepository;
        public RecoverService(IRecoverRepository recoverRepository)
        {
            _recoverRepository = recoverRepository;
        }

        public async Task<ResultsViewModel> RegistDetailRecover(string StartBarcode, string EndBarcode, string user, string ServiceCompanyList, bool PermissionLevel)
        {
            var response = await _recoverRepository.RegistDetailRecover(StartBarcode, EndBarcode, user, ServiceCompanyList, PermissionLevel);
            try
            {
                ResultsViewModel viewModel = await _recoverRepository.RegistDetailRecover(StartBarcode, EndBarcode, user, ServiceCompanyList, PermissionLevel);
                if (viewModel != null && viewModel.Results != null && viewModel.Results.Error.ToUpper() != "SUCCESS" && viewModel.Results.Error.ToUpper() != "NOTSUCCESS")
                {
                    throw new ControledErrorException(viewModel.Results.Error.ToString());
                }
                return viewModel;
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

        public async Task<ResultsViewModel> RegistPartialRecover(string StartBarcode, string EndBarcode, string user, string ServiceCompanyList, bool PermissionLevel)
        {
            try
            {
                ResultsViewModel viewModel = await _recoverRepository.RegistPartialRecover(StartBarcode, EndBarcode, user, ServiceCompanyList, PermissionLevel);
                if (viewModel != null && viewModel.Results != null && viewModel.Results.Error.ToUpper() != "SUCCESS" && viewModel.Results.Error.ToUpper() != "NOTSUCCESS")
                {
                    throw new ControledErrorException(viewModel.Results.Error.ToString());
                }
                return viewModel;
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

        public async Task<ResultsViewModel> RegistTotalRecover(string FileBarcode, string user, string ServiceCompanyList, bool PermissionLevel)
        {
            try
            {
                ResultsViewModel viewModel = await _recoverRepository.RegistTotalRecover(FileBarcode, user, ServiceCompanyList, PermissionLevel);
                if (viewModel != null && viewModel.Results != null && viewModel.Results.Error.ToUpper() != "SUCCESS" && viewModel.Results.Error.ToUpper() != "NOTSUCCESS")
                {
                    throw new ControledErrorException(viewModel.Results.Error.ToString());
                }
                return viewModel;
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
