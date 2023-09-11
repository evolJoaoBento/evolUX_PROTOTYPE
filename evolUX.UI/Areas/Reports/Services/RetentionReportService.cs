using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.Areas.Reports;
using Flurl.Http;
using System.Data;
using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using evolUX.UI.Exceptions;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using Shared.Models.Areas.Finishing;
using evolUX.UI.Areas.Reports.Services.Interfaces;
using evolUX.UI.Areas.Reports.Repositories.Interfaces;
using Microsoft.VisualBasic;

namespace evolUX.UI.Areas.Reports.Services
{
    public class RetentionReportService : IRetentionReportService
    {
        private readonly IRetentionReportRepository _retentionReportRepository;
        public RetentionReportService(IRetentionReportRepository retentionReportRepository)
        {
            _retentionReportRepository = retentionReportRepository;
        }
        public async Task<RetentionRunReportViewModel> GetRetentionRunReport(int BusinessAreaID, int RefDate)
        {
            try
            {
                RetentionRunReportViewModel viewModel = await _retentionReportRepository.GetRetentionRunReport(BusinessAreaID, RefDate);
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

        public async Task<RetentionReportViewModel> GetRetentionReport(List<int> runIDList, int businessAreaID)
        {
            try
            {
                var response = await _retentionReportRepository.GetRetentionReport(runIDList, businessAreaID);
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

        public async Task<RetentionInfoReportViewModel> GetRetentionInfoReport(int RunID, int FileID, int SetID, int DocID)
        {
            try
            {
                var response = await _retentionReportRepository.GetRetentionInfoReport(RunID, FileID, SetID, DocID);
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
