using Flurl.Http;

namespace evolUX.UI.Repositories
{
    public interface IDocCodeRepository
    {
        public Task<IFlurlResponse> GetDocCode();
    }
}