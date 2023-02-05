using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IProductionReportService
    {
        public Task<ProductionRunReportViewModel> GetProductionRunReport(int ServiceCompanyID);
        public Task<ProductionReportViewModel> GetProductionReport(string profileList, int runID, int serviceCompanyID);
        public Task<ProductionReportPrinterViewModel> GetProductionPrinterReport(string profileList, int runID, int serviceCompanyID);
    }
}
