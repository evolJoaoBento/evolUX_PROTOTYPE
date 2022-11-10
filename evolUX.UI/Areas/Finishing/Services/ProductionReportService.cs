using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.API.Areas.Finishing.ViewModels;
using evolUX.UI.Repositories;
using Flurl.Http;
using System.Data;

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
        public async Task<ProductionReportViewModel> GetProductionReport(int runID, int serviceCompanyID)
        {
            var response = await _productionReportRepository.GetProductionReport(runID, serviceCompanyID);
            return response;
        }
    }
}
