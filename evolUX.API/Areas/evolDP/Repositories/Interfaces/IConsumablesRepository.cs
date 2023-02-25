namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IConsumablesRepository
    {
        public Task<List<dynamic>> GetEnvelopeMedia();

        public Task<List<dynamic>> GetEnvelopeMediaGroups();
    }
}
