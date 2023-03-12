using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Shared.Exceptions;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.General;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class ConcludedPrintingService : IConcludedPrintService
    {
        private readonly IConcludedPrintRepository _concludedPrintRepository;
        public ConcludedPrintingService(IConcludedPrintRepository concludedPrintRepository)
        {
            _concludedPrintRepository = concludedPrintRepository;
        }
        public async Task<ResultsViewModel> RegistPrint(string FileBarcode, string user, string ServiceCompanyList)
        {
            try
            {
                ResultsViewModel viewModel = await _concludedPrintRepository.RegistPrint(FileBarcode, user, ServiceCompanyList);
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
