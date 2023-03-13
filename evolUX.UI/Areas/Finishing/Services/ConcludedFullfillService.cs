using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Exceptions;
using evolUX.UI.Repositories;
using Flurl.Http;
using Shared.Exceptions;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.General;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class ConcludedFullfillService : IConcludedFullfillService
    {
        private readonly IConcludedFullfillRepository _concludedFullfillRepository;
        public ConcludedFullfillService(IConcludedFullfillRepository concludedFullfillRepository)
        {
            _concludedFullfillRepository = concludedFullfillRepository;
        }
        public async Task<ResultsViewModel> RegistFullFill(string FileBarcode, string user, string ServiceCompanyList)
        {
            try
            {
                ResultsViewModel viewModel = await _concludedFullfillRepository.RegistFullFill(FileBarcode, user, ServiceCompanyList);
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
