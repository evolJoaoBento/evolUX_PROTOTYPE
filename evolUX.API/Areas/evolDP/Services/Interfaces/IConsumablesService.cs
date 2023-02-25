namespace evolUX.API.Areas.evolDP.Services.Interfaces
{
    public interface IConsumablesService
    {
        public Task<List<dynamic>> GetEnvelopeMedia();
        public Task<List<dynamic>> GetEnvelopeMediaGroups();
    }
}
