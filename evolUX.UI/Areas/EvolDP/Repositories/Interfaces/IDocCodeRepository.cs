using Flurl.Http;

namespace evolUX.UI.Areas.EvolDP.Repositories.Interfaces
{
    public interface IDocCodeRepository
    {
        public Task<IFlurlResponse> GetDocCode();
    }
}