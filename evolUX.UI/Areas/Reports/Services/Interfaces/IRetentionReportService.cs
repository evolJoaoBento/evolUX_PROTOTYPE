using Shared.ViewModels.Areas.Reports;

namespace evolUX.UI.Areas.Reports.Services.Interfaces
{
    public interface IRetentionReportService
    {
        public Task<RetentionRunReportViewModel> GetRetentionRunReport(int BusinessAreaID, int RefDate);
        public Task<RetentionReportViewModel> GetRetentionReport(List<int> runIDList, int businessAreaID);
        public Task<RetentionInfoReportViewModel> GetRetentionInfoReport(int RunID, int FileID, int SetID, int DocID);
    }
}
