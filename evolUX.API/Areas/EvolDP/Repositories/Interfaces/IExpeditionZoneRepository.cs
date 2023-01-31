namespace evolUX.API.Areas.EvolDP.Repositories.Interfaces
{
    public interface IExpeditionZoneRepository
    {
        public Task<List<dynamic>> GetExpeditionZones();
    }
}
