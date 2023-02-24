using Shared.ViewModels.Areas.evolDP;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using evolUX.UI.Areas.evolDP.Repositories.Interfaces;

namespace evolUX.UI.Areas.evolDP.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _projectRepository;
        public ClientService(IClientRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<BusinessViewModel> GetCompanyBusiness(string CompanyBusinessList)
        {
            try
            {
                var response = await _projectRepository.GetCompanyBusiness(CompanyBusinessList);
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
        public async Task<ProjectListViewModel> GetProjects(string CompanyBusinessList)
        {
            try
            {
                var response = await _projectRepository.GetProjects(CompanyBusinessList);
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

