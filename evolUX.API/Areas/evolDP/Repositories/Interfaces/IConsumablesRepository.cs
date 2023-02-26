using Shared.Models.Areas.evolDP;

namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IConsumablesRepository
    {
        public Task<IEnumerable<EnvelopeMediaGroup>> GetEnvelopeMediaGroups(int? envMediaGroupID);
        public Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMedia(int? envMediaID);
    }
}
