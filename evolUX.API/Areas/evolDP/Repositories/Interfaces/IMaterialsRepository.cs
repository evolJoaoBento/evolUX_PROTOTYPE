using Shared.Models.Areas.evolDP;

namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IMaterialsRepository
    {
        public Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes(string fullFillMaterialCode);
        public Task<IEnumerable<MaterialType>> GetMaterialTypes();
        public Task<IEnumerable<EnvelopeMediaGroup>> GetEnvelopeMediaGroups(int? envMediaGroupID);
        public Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMedia(int? envMediaID);
    }
}
