using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IProductionReportService
    {
        public Task<IEnumerable<ProductionRunInfo>> GetProductionRunReport(DataTable ServiceCompanyList);
        public Task<ProductionReportViewModel> GetProductionReport(int runID, int serviceCompanyID);
    }
}
