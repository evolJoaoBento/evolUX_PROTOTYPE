using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using Flurl.Http;

namespace evolUX.UI.Areas.evolDP.Services
{
    public class ExpeditionService : IExpeditionService
    {
        private readonly IExpeditionRepository _expeditionTypeRepository;

        public ExpeditionService(IExpeditionRepository expeditionTypeRepository)
        {
            _expeditionTypeRepository = expeditionTypeRepository;
        }
        public async Task<IFlurlResponse> GetExpeditionTypes()
        {
            var response = await _expeditionTypeRepository.GetExpeditionTypes();
            return response;
        }
    }
}
