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
            
            IEnumerable<ProductionInfo> productionReport = await _repository.ProductionReport.GetProductionDetailReport(runID,  serviceCompanyID, paperMediaID, stationMediaID,  expeditionType, expCode, hasColorPages);
            if (productionReport == null)
            {

            }
           

            return productionReport;
        }

        public async Task<ProductionReportViewModel> GetProductionReport(int runID, int serviceCompanyID)
        {
            IEnumerable<ProductionDetailInfo> productionReport = await _repository.ProductionReport.GetProductionReport(runID, serviceCompanyID);
            if (productionReport == null)
            {

            }
            string serviceCompanyCode = await _repository.ProductionReport.GetServiceCompanyCode(serviceCompanyID);
            if (serviceCompanyCode == null)
            {

            }
            ProductionReportViewModel viewmodel = new ProductionReportViewModel();
            viewmodel.ProductionReport = productionReport;
            viewmodel.ServiceCompanyCode = serviceCompanyCode;
            foreach (ProductionDetailInfo pdi in viewmodel.ProductionReport)
            {
                pdi.ProductionDetailReport = await _repository.ProductionReport.GetProductionDetailReport(runID, serviceCompanyID, pdi.PaperMediaID, pdi.StationMediaID, pdi.ExpeditionType, pdi.ExpCode, pdi.HasColorPages);
            }
            return viewmodel;
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
