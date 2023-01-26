using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;
using evolUX.UI.Repositories.Interfaces;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class ProductionReportService : IProductionReportService
    {
        private readonly IProductionReportRepository _productionReportRepository;
        public ProductionReportService(IProductionReportRepository productionReportRepository)
        {
            _productionReportRepository = productionReportRepository;
        }
        public async Task<ProductionRunReportViewModel> GetProductionRunReport(string ServiceCompanyList)
        {
            var response = await _productionReportRepository.GetProductionRunReport(ServiceCompanyList);
            return response;
        }
        public async Task<ProductionReportViewModel> GetProductionReport(string profileList, int runID, int serviceCompanyID)
        {
            var response = await _productionReportRepository.GetProductionReport(profileList, runID, serviceCompanyID);
            return response;
        }
    }
}
