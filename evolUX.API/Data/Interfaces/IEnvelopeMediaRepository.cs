

namespace evolUX.API.Data.Interfaces
{
    public interface IEnvelopeMediaRepository
    {
        public Task<List<dynamic>> GetEnvelopeMedia();

        public Task<List<dynamic>> GetEnvelopeMediaGroups();
    }
}
