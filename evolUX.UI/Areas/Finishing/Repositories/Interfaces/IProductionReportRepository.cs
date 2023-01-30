using Flurl.Http;
using Shared.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Repositories.Interfaces
{
    public interface IProductionReportRepository
    {
        public Task<ProductionRunReportViewModel> GetProductionRunReport(string ServiceCompanyList);
        public Task<ProductionReportViewModel> GetProductionReport(string profileList, int runID, int serviceCompanyID);
    }
}