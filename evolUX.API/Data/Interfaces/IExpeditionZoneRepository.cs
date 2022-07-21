

namespace evolUX.API.Data.Interfaces
{
    public interface IExpeditionZoneRepository
    {
        public Task<List<dynamic>> GetExpeditionZones();
    }
}
