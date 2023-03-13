using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;
using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Exceptions;
using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.Core;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class PendingRegistService : IPendingRegistService
    {
        private readonly IPendingRegistRepository _pendingRegistRepository;
        public PendingRegistService(IPendingRegistRepository pendingRegistRepository)
        {
            _pendingRegistRepository = pendingRegistRepository;
        }
        public async Task<PendingRegistViewModel> GetPendingRegist(string ServiceCompanyList)
        {
            try
            {
                var response = await _pendingRegistRepository.GetPendingRegist(ServiceCompanyList);
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
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                throw new ErrorViewModelException(viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.HResult;
                viewModel.ErrorResult.Message = ex.Message;
                throw new ErrorViewModelException(viewModel);
            }
        }
        public async Task<PendingRegistDetailViewModel> GetPendingRegistDetail(int runID, string ServiceCompanyList)
        {
            try
            {
                var response = await _pendingRegistRepository.GetPendingRegistDetail(runID, ServiceCompanyList);
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
                viewModel.ErrorResult.Code = (int)ex.StatusCode;
                viewModel.ErrorResult.Message = ex.Message;
                throw new ErrorViewModelException(viewModel);
            }
            catch (HttpNotFoundException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Code = (int)ex.HResult;
                viewModel.ErrorResult.Message = ex.Message;
                throw new ErrorViewModelException(viewModel);
            }
        }
    }
}
