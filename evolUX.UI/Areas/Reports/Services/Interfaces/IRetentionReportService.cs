using Shared.ViewModels.Areas.Reports;
using Flurl.Http;
using System.Data;
using Shared.Models.Areas.Finishing;

namespace evolUX.UI.Areas.Reports.Services.Interfaces
{
    public interface IRetentionReportService
    {
        public Task<RetentionRunReportViewModel> GetRetentionRunReport(int BusinessAreaID, DateTime DateRef);
        public Task<RetentionReportViewModel> GetRetentionReport(List<int> runIDList, int businessAreaID);
    }
}
