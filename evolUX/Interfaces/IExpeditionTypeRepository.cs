using evolUX.Models;

namespace evolUX.Interfaces
{
    public interface IExpeditionTypeRepository
    {
        public Task<List<dynamic>> GetExpeditionTypes();
    }
}
