using Flurl.Http;

namespace evolUX.UI.Areas.evolDP.Repositories.Interfaces
{
    public interface IExpeditionTypeRepository
    {
        public Task<IFlurlResponse> GetExpeditionTypes();
    }
}
