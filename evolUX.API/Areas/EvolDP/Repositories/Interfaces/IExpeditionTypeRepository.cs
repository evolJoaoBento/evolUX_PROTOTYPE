namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IExpeditionTypeRepository
    {
        public Task<List<dynamic>> GetExpeditionTypes();
    }
}
