using Flurl.Http;
using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using Shared.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Repositories.Interfaces
{
    public interface IExpeditionReportRepository
    {
        public Task<BusinessViewModel> GetCompanyBusiness(string CompanyBusinessList);
        public Task<ExpeditionListViewModel> GetPendingExpeditionFiles(int BusinessID, string ServiceCompanyList);
        public Task<Result> RegistExpeditionReport(List<RegistExpReportElement> expFiles, string username, int userID);
        public Task<ExpeditionListViewModel> GetExpeditionReportList(int BusinessID, string ServiceCompanyList);

    }
}