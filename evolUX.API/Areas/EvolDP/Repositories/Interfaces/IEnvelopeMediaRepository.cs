namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IEnvelopeMediaRepository
    {
        public Task<List<dynamic>> GetEnvelopeMedia();

        public Task<List<dynamic>> GetEnvelopeMediaGroups();
    }
}
