using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services
{
    public class ProductionReportService : IProductionReportService
    {
        private class PrintersSpecs
        {
            public string Specs { get; set; }
            public IEnumerable<ResourceInfo> List { get; set; }
            public PrintersSpecs() 
            {
                Specs = string.Empty;
                List = new List<ResourceInfo>();
            }
        }
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

        public async Task<ProductionReportViewModel> GetProductionReport(IEnumerable<int> profileList, int runID, int serviceCompanyID)
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
            List<PrintersSpecs> printersList = new List<PrintersSpecs>();
            bool existsToPrintFiles = false;
            foreach (ProductionDetailInfo pdi in viewmodel.ProductionReport)
            {
                pdi.ProductionDetailReport = await _repository.ProductionReport.GetProductionDetailReport(runID, serviceCompanyID, pdi.PaperMediaID, pdi.StationMediaID, pdi.ExpeditionType, pdi.ExpCode, pdi.HasColorPages);
                foreach (ProductionInfo f in pdi.ProductionDetailReport)
                {
                    if (!f.FilePrintedFlag || !f.RegistDetailFilePrintedFlag)
                        existsToPrintFiles = true;
                    if (!string.IsNullOrEmpty(f.FilePrinterSpecs) && !f.FilePrintedFlag)
                    {
                        PrintersSpecs pList = printersList.Find(x => x.Specs.Contains(f.FilePrinterSpecs));
                        if (pList != null)
                            f.FilePrinters = pList.List;
                        else
                        {
                            f.FilePrinters = await _repository.RegistJob.GetResources("PRINTER", profileList, f.FilePrinterSpecs, false);
                            printersList.Add(new PrintersSpecs { Specs = f.FilePrinterSpecs, List = f.FilePrinters});
                        }
                    }
                    if (!string.IsNullOrEmpty(f.RegistDetailFilePrinterSpecs) && !f.RegistDetailFilePrintedFlag)
                    {
                        PrintersSpecs pList = printersList.Find(x => x.Specs.Contains(f.RegistDetailFilePrinterSpecs));
                        if (pList != null)
                            f.RegistDetailFilePrinters = pList.List;
                        else
                        {
                            f.RegistDetailFilePrinters = await _repository.RegistJob.GetResources("PRINTER", profileList, f.RegistDetailFilePrinterSpecs, false);
                            printersList.Add(new PrintersSpecs { Specs = f.FilePrinterSpecs, List = f.RegistDetailFilePrinters });
                        }
                    }
                }

            }
            if (existsToPrintFiles)
                viewmodel.Resources = await _repository.RegistJob.GetResources("PRINTER", profileList, "", false);

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
