using Microsoft.IdentityModel.Tokens;
using SharedModels.ViewModels.Areas.Core;
using System.Runtime.Serialization;

namespace Extensions.Exceptions
{
    [Serializable]
    internal class ErrorViewModelException : Exception
    {
        ErrorViewModel ViewModel { get; set; }
        public ErrorViewModelException()
        {
            ViewModel = new ErrorViewModel();
        }

        public ErrorViewModelException(string? message) : base(message)
        {
            ViewModel = new ErrorViewModel();
            if (message != null)
                ViewModel.ErrorResult.Message = message;
        }

        public ErrorViewModelException(ErrorViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public ErrorViewModelException(string? message, Exception? innerException) : base(message, innerException)
        {
            ViewModel = new ErrorViewModel();
            if (message != null)
                ViewModel.ErrorResult.Message = message;
        }

        protected ErrorViewModelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ViewModel = new ErrorViewModel();
        }
    }
}