using Flurl.Http;

namespace evolUX.UI.Areas.evolDP.Repositories.Interfaces
{
    public interface IExpeditionRepository
    {
        public Task<IFlurlResponse> GetExpeditionTypes();
    }
}
