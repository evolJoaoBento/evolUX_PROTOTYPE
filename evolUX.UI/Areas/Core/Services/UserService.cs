using evolUX.UI.Areas.Core.Models;
using evolUX.UI.Areas.Core.Services.Interfaces;
using evolUX.UI.Exceptions;
using evolUX.UI.Repositories;
using evolUX.UI.Repositories.Interfaces;
using Flurl.Http;
using Shared.Models.Areas.Core;
using Shared.Models.General;
using Shared.ViewModels.Areas.Core;
using System.Net;
using System.Reflection;
using System.Xml.Linq;

namespace evolUX.UI.Areas.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        

        public async Task ChangeCulture(string culture){
            try
            {
                await _userRepository.ChangeCulture(culture);
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
                viewModel.ErrorResult.StackTrace = ex.StackTrace;
                throw new ErrorViewModelException(viewModel);
            }
            catch(Exception ex)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.RequestID = ex.Source;
                viewModel.ErrorResult = new ErrorResult();
                viewModel.ErrorResult.Message = ex.Message;
                viewModel.ErrorResult.StackTrace = ex.StackTrace;
                throw new ErrorViewModelException(viewModel);
            }
        }
    }
}
