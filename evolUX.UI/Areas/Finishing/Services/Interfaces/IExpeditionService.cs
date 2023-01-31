using Shared.ViewModels.Areas.Finishing;
using Shared.ViewModels.Areas.evolDP;
using Shared.Models.Areas.Finishing;
using Shared.Models.General;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IExpeditionService
    {
        public Task<BusinessViewModel> GetCompanyBusiness(string CompanyBusinessList);
        public Task<ExpeditionFilesViewModel> GetPendingExpeditionFiles(int BusinessID, string ServiceCompanyList);
        public Task<Result> RegistExpeditionReport(List<RegistExpReportElement> expFiles, string username, int userID);
    }
}
