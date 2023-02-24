namespace evolUX.API.Areas.evolDP.Services.Interfaces
{
    public interface IEnvelopeMediaService
    {
        public Task<List<dynamic>> GetEnvelopeMedia();
        public Task<List<dynamic>> GetEnvelopeMediaGroups();
    }
}
