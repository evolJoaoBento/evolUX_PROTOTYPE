using SharedModels.ViewModels.Areas.Finishing;
using SharedModels.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IProductionReportService
    {
        public Task<IEnumerable<ProductionRunInfo>> GetProductionRunReport(DataTable ServiceCompanyList);
        public Task<IEnumerable<ProductionDetailInfo>> GetProductionReport(int runID, int serviceCompanyID);
        public Task<IEnumerable<ProductionInfo>> GetProductionDetailReport(int runID, int serviceCompanyID, int paperMediaID, int stationMediaID, int expeditionType, string expCode, bool hasColorPages);
    }
}
