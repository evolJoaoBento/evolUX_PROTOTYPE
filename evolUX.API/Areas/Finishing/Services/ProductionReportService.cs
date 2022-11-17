using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services
{
    public class ProductionReportService : IProductionReportService
    {
        private readonly IWrapperRepository _repository;
        public ProductionReportService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductionInfo>> GetProductionDetailReport(int runID, int serviceCompanyID, int paperMediaID, int stationMediaID, int expeditionType, string expCode, bool hasColorPages)
        {
            IEnumerable<ProductionInfo> productionRunReport = await _repository.ProductionReport.GetProductionDetailReport(runID,  serviceCompanyID, paperMediaID, stationMediaID,  expeditionType, expCode, hasColorPages);
            if (productionRunReport == null)
            {

            }
            return productionRunReport;
        }

        public async Task<IEnumerable<ProductionDetailInfo>> GetProductionReport(int runID, int serviceCompanyID)
        {
            IEnumerable<ProductionDetailInfo> productionRunReport = await _repository.ProductionReport.GetProductionReport(runID, serviceCompanyID);
            if (productionRunReport == null)
            {

            }
            return productionRunReport;
        }

        public async Task<IEnumerable<ProductionRunInfo>> GetProductionRunReport(DataTable ServiceCompanyList)
        {
            IEnumerable<ProductionRunInfo> productionRunReport = await _repository.ProductionReport.GetProductionRunReport(ServiceCompanyList);
            if (productionRunReport == null)
            {

            }
            return productionRunReport;
        }
    }
}
