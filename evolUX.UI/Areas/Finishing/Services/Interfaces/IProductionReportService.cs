using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;
using Shared.Models.Areas.Finishing;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IProductionReportService
    {
        public Task<ProductionRunReportViewModel> GetProductionRunReport(int ServiceCompanyID);
        public Task<ProductionReportViewModel> GetProductionReport(string profileList, List<int> runIDList, int serviceCompanyID, bool filterOnlyPrint);
        public Task<IEnumerable<ProductionDetailInfo>> GetProductionReportFilters(string profileList, List<int> runIDList, int serviceCompanyID, bool filterOnlyPrint);
    }
}
