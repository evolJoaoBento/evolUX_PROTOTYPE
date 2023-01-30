using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IExpeditionService
    {
        public Task<BusinessViewModel> GetCompanyBusiness(string CompanyBusinessList);
        public Task<ExpeditionFilesViewModel> GetPendingExpeditionFiles(int BusinessID, string ServiceCompanyList);
    }
}
