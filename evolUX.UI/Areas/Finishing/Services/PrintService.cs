﻿using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Exceptions;
using evolUX.UI.Repositories;
using Flurl.Http;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.General;
using System.Data;
using System.Reflection;
using Shared.Models.Areas.Core;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class PrintService : IPrintService
    {
        private readonly IPrintRepository _printRepository;
        public PrintService(IPrintRepository printRepository)
        {
            _printRepository = printRepository;
        }
        public async Task<ResoursesViewModel> GetPrinters(string profileList, string filesSpecs, bool ignoreProfiles)
        {
            try
            {
                var response = await _printRepository.GetPrinters(profileList, filesSpecs, ignoreProfiles);
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

        public async Task<ResultsViewModel> Print(int runID, int fileID, string printer, string serviceCompanyCode,
            string username, int userID, string filePath, string fileName, string shortFileName)
        {
            try
            {
                ResultsViewModel response = await _printRepository.Print(runID, fileID, printer, serviceCompanyCode,
                            username, userID, filePath, fileName, shortFileName);
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
