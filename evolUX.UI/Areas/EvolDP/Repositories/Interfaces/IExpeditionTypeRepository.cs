using Flurl.Http;

namespace evolUX.UI.Areas.EvolDP.Repositories.Interfaces
{
    public interface IExpeditionTypeRepository
    {
        public Task<IFlurlResponse> GetExpeditionTypes();
    }
}
