using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IProductionReportService
    {
        public Task<IEnumerable<ProductionRunInfo>> GetProductionRunReport(int ServiceCompanyID);
        public Task<ProductionReportViewModel> GetProductionReport(IEnumerable<int> profileList, int runID, int serviceCompanyID);
        public Task<ProductionReportPrinterViewModel> GetProductionPrinterReport(IEnumerable<int> profileList, int runID, int serviceCompanyID);
    }
}
