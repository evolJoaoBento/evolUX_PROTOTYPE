namespace evolUX.API.Areas.EvolDP.Repositories.Interfaces
{
    public interface IExpeditionTypeRepository
    {
        public Task<List<dynamic>> GetExpeditionTypes();
    }
}
