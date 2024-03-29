﻿using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.General;
using System.Data;
using System.Reflection;
using Shared.Models.Areas.Core;
using Shared.Models.General;
using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using System.Xml;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class PrintService : IPrintService
    {
        private readonly IPrintRepository _printRepository;
        public PrintService(IPrintRepository printRepository)
        {
            _printRepository = printRepository;
        }
        public async Task<PrinterViewModel> GetPrinters(string profileList, string filesSpecs, bool ignoreProfiles)
        {
            try
            {
                PrinterViewModel viewModel = await _printRepository.GetPrinters(profileList, filesSpecs, ignoreProfiles);
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

        public async Task<Result> Print(string printer, string serviceCompanyCode,
            string username, int userID, List<PrintFileInfo> prodFiles)
        {
            try
            {
                Result response = await _printRepository.Print(printer, serviceCompanyCode, username, userID, prodFiles);
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
                viewModel.ErrorResult.StackTrace = ex.StackTrace;
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
