using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Areas.Core.Repositories.Interfaces;
using evolUX.UI.Areas.Core.Services.Interfaces;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using System.Net;
using System.Xml.Linq;

namespace evolUX.UI.Areas.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<IFlurlResponse> GetTokenAndUser(string username)
        {
            try 
            { 
                var response = await _authRepository.GetTokenAndUser(username);
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
                if (viewModel.ErrorResult.Code == 503)
                    viewModel.ErrorResult.Message = "There was a problem accessing the database! Please try again later.";
                else
                    viewModel.ErrorResult.Message = ex.Message;
                throw new ErrorViewModelException(viewModel);
            }
        }

        public async Task<IFlurlResponse> LoginCredentials(string username, string password)
        {
            try
            {
                var response = await _authRepository.LoginCredentials(username, password);
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
                if (viewModel.ErrorResult.Code == 503)
                    viewModel.ErrorResult.Message = "There was a problem accessing the database! Please try again later.";
                else
                    viewModel.ErrorResult.Message = ex.Message;
                throw new ErrorViewModelException(viewModel);
            }
        }

        public async Task<IFlurlResponse> GetRefreshToken(string accessToken, string refreshToken)
        {
            try
            {
                var response = await _authRepository.GetRefreshToken(accessToken, refreshToken);
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
                if (viewModel.ErrorResult.Code == 503)
                    viewModel.ErrorResult.Message = "There was a problem accessing the database! Please try again later.";
                else
                    viewModel.ErrorResult.Message = ex.Message;
                throw new ErrorViewModelException(viewModel);
            }
        }

        public async Task<Dictionary<string, string>> GetSessionVariables(int ID)
        {
            try
            {
                var response = await _authRepository.GetSessionVariables(ID);
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
                if (viewModel.ErrorResult.Code == 503)
                    viewModel.ErrorResult.Message = "There was a problem accessing the database! Please try again later.";
                else
                    viewModel.ErrorResult.Message = ex.Message;
                throw new ErrorViewModelException(viewModel);
            }
            catch(UnauthorizedAccessException ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorResult.Message = "The user has no profiles registered!";
                throw new ErrorViewModelException(viewModel);
            }
        }
    }
}
