namespace evolUX.API.Areas.EvolDP.Services.Interfaces
{
    public interface IEnvelopeMediaService
    {
        public Task<List<dynamic>> GetEnvelopeMedia();
        public Task<List<dynamic>> GetEnvelopeMediaGroups();
    }
}
