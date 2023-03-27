using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using Shared.Models.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Services
{
    public class ConsumablesService : IConsumablesService
    {
        private readonly IWrapperRepository _repository;

        public ConsumablesService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes(string fullFillMaterialCode)
        {
            IEnumerable<FulfillMaterialCode> result = await _repository.Consumables.GetFulfillMaterialCodes(fullFillMaterialCode);
            return result;
        }
        public async Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMedia(int? envMediaID)
        {
            var envelopeMediaList = await _repository.Consumables.GetEnvelopeMedia(envMediaID);
            if (envelopeMediaList == null)
            {

            }
            return envelopeMediaList;
        }        
        public async Task<IEnumerable<EnvelopeMediaGroup>> GetEnvelopeMediaGroups(int? envMediaGroupID)
        {
            var envelopeMediaGroupList = await _repository.Consumables.GetEnvelopeMediaGroups(envMediaGroupID);
            if (envelopeMediaGroupList == null)
            {

            }
            return envelopeMediaGroupList;
        }
    }
}
