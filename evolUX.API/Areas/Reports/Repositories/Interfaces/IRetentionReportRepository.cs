using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.Models.Areas.Reports;
using System.Data;

namespace evolUX.API.Areas.Reports.Repositories.Interfaces
{
    public interface IRetentionReportRepository
    {
        public Task<IEnumerable<RetentionRunInfo>> GetRetentionRunReport(int BusinessAreaID, DateTime DateRef);
        //public Task<IEnumerable<ProdFileInfo>> GetRetentionDetailReport(DataTable runIDList, int businessAreaID);
        //public Task<IEnumerable<RetentionDetailInfo>> GetRetentionReport(DataTable runIDList, int businessAreaID);
        //public Task<string> GetBusinessAreaCode(int businessAreaID);

    }
}
