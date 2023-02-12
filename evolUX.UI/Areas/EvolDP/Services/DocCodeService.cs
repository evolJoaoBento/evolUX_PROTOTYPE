using evolUX.API.Models;
using evolUX.UI.Areas.EvolDP.Repositories.Interfaces;
using evolUX.UI.Areas.EvolDP.Services.Interfaces;
using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

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
        public async Task<DocCodeViewModel> GetDocCodeConfig(int docCodeID)
        {
            var response = await _docCodeRepository.GetDocCodeConfig(docCodeID);
            return response;
        }
        public async Task<DocCodeConfigOptionsViewModel> GetDocCodeConfigOptions(DocCode? docCode)
        {
            var response = await _docCodeRepository.GetDocCodeConfigOptions(docCode);
            return response;
        }
    }
}
