using Flurl.Http;
using Shared.Models.Areas.Finishing;
using Shared.ViewModels.Areas.Finishing;
using System.Data;
using evolUX.UI.Areas.Reports.Repositories;
using Shared.ViewModels.Areas.Reports;

namespace evolUX.UI.Areas.Reports.Repositories.Interfaces
{
    public interface IRetentionReportRepository
    {
        public Task<RetentionRunReportViewModel> GetRetentionRunReport(int BusinessAreaID, DateTime DateRef);
        public Task<RetentionReportViewModel> GetRetentionReport(List<int> runIDList, int businessAreaID);
        public Task<RetentionInfoReportViewModel> GetRetentionInfoReport(List<int> runIDList, int businessAreaID);
    }
}