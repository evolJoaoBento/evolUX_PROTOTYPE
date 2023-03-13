using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;
using Shared.Models.General;
using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Areas.Finishing.Repositories;
using evolUX.UI.Exceptions;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class PendingRecoverService : IPendingRecoverService
    {
        private readonly IPendingRecoverRepository _pendingRecoverRepository;
        public PendingRecoverService(IPendingRecoverRepository pendingRecoverRepository)
        {
            _pendingRecoverRepository = pendingRecoverRepository;
        }
        public async Task<ServiceCompaniesViewModel> GetServiceCompanies(string ServiceCompanyList)
        {
            try
            {
                var response = await _pendingRecoverRepository.GetServiceCompanies(ServiceCompanyList);
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
        public async Task<PendingRecoverDetailViewModel> GetPendingRecoveries(int serviceCompanyID, string serviceCompanyCode)
        {
            try
            {
                var response = await _pendingRecoverRepository.GetPendingRecoveries(serviceCompanyID, serviceCompanyCode);
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
        public async Task<Result> RegistPendingRecover(int serviceCompanyID, string serviceCompanyCode, string recoverType, int userid)
        {
            try
            {
                var response = await _pendingRecoverRepository.RegistPendingRecover(serviceCompanyID, serviceCompanyCode, recoverType, userid);
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
