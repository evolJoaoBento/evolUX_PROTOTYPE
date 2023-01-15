using Flurl.Http;

namespace evolUX.UI.Repositories.Interfaces
{
    public interface IDocCodeRepository
    {
        public Task<IFlurlResponse> GetDocCode();
    }
}