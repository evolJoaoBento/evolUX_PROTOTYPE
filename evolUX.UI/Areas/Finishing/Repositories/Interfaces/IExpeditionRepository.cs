using Flurl.Http;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using Shared.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Repositories.Interfaces
{
    public interface IExpeditionRepository
    {
        public Task<BusinessViewModel> GetCompanyBusiness(string CompanyBusinessList);
        public Task<ExpeditionFilesViewModel> GetPendingExpeditionFiles(int BusinessID, string ServiceCompanyList);
    }
}