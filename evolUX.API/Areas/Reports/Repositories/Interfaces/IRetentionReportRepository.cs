using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.Models.Areas.Reports;
using System.Data;

namespace evolUX.API.Areas.Reports.Repositories.Interfaces
{
    public interface IRetentionReportRepository
    {
        public Task<IEnumerable<RetentionRunInfo>> GetRetentionRunReport(int BusinessAreaID, int RefDate);
        //public Task<IEnumerable<ProdFileInfo>> GetRetentionDetailReport(DataTable runIDList, int businessAreaID);
        public Task<IEnumerable<RetentionInfo>> GetRetentionReport(DataTable runIDList, int BusinessAreaID);
        //public Task<string> GetBusinessAreaCode(int businessAreaID);

    }
}
