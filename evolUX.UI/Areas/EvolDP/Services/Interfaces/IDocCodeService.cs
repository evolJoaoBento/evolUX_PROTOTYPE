using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.EvolDP.Services.Interfaces
{
    public interface IDocCodeService
    {
        public Task<DocCodeViewModel> GetDocCodeGroup();
        public Task<DocCodeViewModel> GetDocCode(string docLayout, string docType);
        public Task<DocCodeViewModel> GetDocCodeConfig(int docCodeID);
        public Task<DocCodeConfigOptionsViewModel> GetDocCodeConfigOptions(DocCode? docCode);
    }
}
