namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IExpeditionZoneRepository
    {
        public Task<List<dynamic>> GetExpeditionZones();
    }
}
