using Flurl.Http;

namespace evolUX.UI.Areas.EvolDP.Services.Interfaces
{
    public interface IExpeditionTypeService
    {
        public Task<IFlurlResponse> GetExpeditionTypes();
    }
}
