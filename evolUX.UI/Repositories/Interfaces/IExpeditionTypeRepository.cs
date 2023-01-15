using Flurl.Http;

namespace evolUX.UI.Repositories.Interfaces
{
    public interface IExpeditionTypeRepository
    {
        public Task<IFlurlResponse> GetExpeditionTypes();
    }
}
