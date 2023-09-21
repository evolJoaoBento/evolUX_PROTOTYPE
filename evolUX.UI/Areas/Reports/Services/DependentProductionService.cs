﻿using Shared.ViewModels.Areas.Reports;
using Flurl.Http;
using evolUX.UI.Exceptions;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using evolUX.UI.Areas.Reports.Repositories.Interfaces;

namespace evolUX.UI.Areas.Reports.Services
{
    public class DependentProductionService
    {
        private readonly IDependentProductionRepository _dependentProductionRepository;
        public DependentProductionService(IDependentProductionRepository dependentProductionRepository)
        {
            _dependentProductionRepository = dependentProductionRepository;
        }
        public async Task<DependentProductionViewModel> GetDependentPrintsProduction(List<int> ServiceCompanyList)
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
