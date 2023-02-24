using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;

namespace evolUX.API.Areas.evolDP.Services
{
    public class ExpeditionZoneService : IExpeditionZoneService
    {
        private readonly IWrapperRepository _repository;
        public ExpeditionZoneService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<dynamic>> GetExpeditionZones()
        {
            var expeditionZoneList = await _repository.ExpeditionZone.GetExpeditionZones();
            if (expeditionZoneList == null)
            {

            }
            return expeditionZoneList;
        }
    }
}
