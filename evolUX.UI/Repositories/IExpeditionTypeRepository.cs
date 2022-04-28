using Flurl.Http;

namespace evolUX.UI.Repositories
{
    public interface IExpeditionTypeRepository
    {
        public Task<IFlurlResponse> GetExpeditionTypes();
    }
}
