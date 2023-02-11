using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.EvolDP.Repositories.Interfaces
{
    public interface IDocCodeRepository
    {
        public Task<DocCodeViewModel> GetDocCodeGroup();
        public Task<DocCodeViewModel> GetDocCode(string docLayout, string docType);
    }
}