using Shared.ViewModels.Areas.Reports;
using Flurl.Http;
using evolUX.UI.Exceptions;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using evolUX.UI.Areas.Reports.Repositories.Interfaces;
using evolUX.UI.Areas.Reports.Services.Interfaces;
using System.Data;

namespace evolUX.UI.Areas.Reports.Services
{
    public class DependentProductionService : IDependentProductionService
    {
        private readonly IDependentProductionRepository _dependentProductionRepository;
        public DependentProductionService(IDependentProductionRepository dependentProductionRepository)
        {
            _dependentProductionRepository = dependentProductionRepository;
        }
        public async Task<DependentProductionViewModel> GetDependentPrintsProduction(DataTable ServiceCompanyList)
        {
            try
            {
                var response = await _dependentProductionRepository.GetDependentPrintsProduction(ServiceCompanyList);
                return response;
            }
            catch (FlurlHttpException ex)
            {
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
