﻿using evolUX.API.Models;
using evolUX.UI.Areas.EvolDP.Repositories.Interfaces;
using evolUX.UI.Areas.EvolDP.Services.Interfaces;
using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using Shared.ViewModels.General;
using System.Data;

namespace evolUX.UI.Areas.EvolDP.Services
{
    public class DocCodeService : IDocCodeService
    {
        private readonly IDocCodeRepository _docCodeRepository;
        public DocCodeService(IDocCodeRepository docCodeRepository)
        {
            _docCodeRepository = docCodeRepository;
        }
        public async Task<DocCodeViewModel> GetDocCodeGroup()
        {
            var response = await _docCodeRepository.GetDocCodeGroup();
            return response;
        }
        public async Task<DocCodeViewModel> GetDocCode(string docLayout, string docType)
        {
            var response = await _docCodeRepository.GetDocCode(docLayout, docType);
            return response;
        }
        public async Task<DocCodeConfigViewModel> GetDocCodeConfig(int docCodeID)
        {
            var response = await _docCodeRepository.GetDocCodeConfig(docCodeID);
            return response;
        }
        public async Task<DocCodeConfigOptionsViewModel> GetDocCodeConfigOptions(DocCode? docCode)
        {
            var response = await _docCodeRepository.GetDocCodeConfigOptions(docCode);
            return response;
        }
        public async Task<DocCodeConfigViewModel> RegistDocCodeConfig(DocCode docCode)
        {
            var response = await _docCodeRepository.RegistDocCodeConfig(docCode);
            return response;
        }
        public async Task<DocCodeConfigViewModel> ChangeDocCode(DocCode docCode)
        {
            var response = await _docCodeRepository.ChangeDocCode(docCode);
            return response;
        }
        public async Task<ResultsViewModel> DeleteDocCodeConfig(int docCodeID, int startDate)
        {
            var response = await _docCodeRepository.DeleteDocCodeConfig(docCodeID, startDate);
            return response;
        }
        public async Task<ResultsViewModel> DeleteDocCode(DocCode docCode)
        {
            var response = await _docCodeRepository.DeleteDocCode(docCode);
            return response;
        }
        public async Task<DocCodeCompatibilityViewModel> GetCompatibility(int docCodeID)
        {
            var response = await _docCodeRepository.GetCompatibility(docCodeID);
            return response;
        }
        public async Task<DocCodeCompatibilityViewModel> ChangeCompatibility(int docCodeID, List<string> docCodeList)
        {
            var response = await _docCodeRepository.ChangeCompatibility(docCodeID, docCodeList);
            return response;
        }
    }
}
