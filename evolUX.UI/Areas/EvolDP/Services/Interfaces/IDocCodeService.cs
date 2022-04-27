using Flurl.Http;

namespace evolUX.UI.Areas.EvolDP.Services.Interfaces
{
    public interface IDocCodeService
    {
        public Task<IFlurlResponse> GetDocCode();
    }
}
