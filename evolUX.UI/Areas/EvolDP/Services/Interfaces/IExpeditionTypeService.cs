using Flurl.Http;

namespace evolUX.UI.Areas.evolDP.Services.Interfaces
{
    public interface IExpeditionTypeService
    {
        public Task<IFlurlResponse> GetExpeditionTypes();
    }
}
