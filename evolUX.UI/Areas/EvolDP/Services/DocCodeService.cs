using evolUX.UI.Areas.EvolDP.Services.Interfaces;
using evolUX.UI.Repositories.Interfaces;
using Flurl.Http;

namespace evolUX.UI.Areas.EvolDP.Services
{
    public class DocCodeService : IDocCodeService
    {
        private readonly IDocCodeRepository _docCodeRepository;
        public DocCodeService(IDocCodeRepository docCodeRepository)
        {
            _docCodeRepository = docCodeRepository;
        }
        public async Task<IFlurlResponse> GetDocCode()
        {
            var response = await _docCodeRepository.GetDocCode();
            return response;
        }
    }
}
