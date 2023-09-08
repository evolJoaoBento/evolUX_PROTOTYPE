using Shared.ViewModels.Areas.Reports;
using Shared.Models.Areas.Reports;
using System.Data;

namespace evolUX.API.Areas.Reports.Services.Interfaces
{
    public interface IRetentionReportService
    {
        public Task<RetentionRunReportViewModel> GetRetentionRunReport(int BusinessAreaID, int RefDate);
        public Task<RetentionReportViewModel> GetRetentionReport(DataTable runIDList, int businessAreaID);
        public Task<RetentionInfoReportViewModel> GetRetentionInfoReport(int RunID, int FileID);
    }
}
