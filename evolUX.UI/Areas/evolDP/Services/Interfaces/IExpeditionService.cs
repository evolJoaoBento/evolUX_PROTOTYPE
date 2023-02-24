using Flurl.Http;

namespace evolUX.UI.Areas.evolDP.Services.Interfaces
{
    public interface IExpeditionService
    {
        public Task<IFlurlResponse> GetExpeditionTypes();
    }
}
