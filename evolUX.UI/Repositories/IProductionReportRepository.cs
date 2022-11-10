using evolUX.API.Areas.Finishing.ViewModels;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Repositories
{
    public interface IProductionReportRepository
    {
        public Task<ProductionRunReportViewModel> GetProductionRunReport(string ServiceCompanyList);
        public Task<ProductionReportViewModel> GetProductionReport(int runID, int serviceCompanyID);
    }
}