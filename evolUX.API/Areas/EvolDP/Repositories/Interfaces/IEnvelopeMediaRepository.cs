namespace evolUX.API.Areas.EvolDP.Repositories.Interfaces
{
    public interface IEnvelopeMediaRepository
    {
        public Task<List<dynamic>> GetEnvelopeMedia();

        public Task<List<dynamic>> GetEnvelopeMediaGroups();
    }
}
