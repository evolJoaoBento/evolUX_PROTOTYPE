

namespace evolUX.API.Data.Interfaces
{
    public interface IExpeditionTypeRepository
    {
        public Task<List<dynamic>> GetExpeditionTypes();
    }
}
