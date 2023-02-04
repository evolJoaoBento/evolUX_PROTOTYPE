using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using System.Data;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using Shared.Models.Areas.Core;
using System.Xml;
using Shared.Models.General;

namespace evolUX.API.Areas.Finishing.Services
{
    public class ProductionReportService : IProductionReportService
    {
        private readonly IWrapperRepository _repository;
        private readonly IPrintService _print;
        public ProductionReportService(IWrapperRepository repository, IPrintService print)
        {
            _repository = repository;
            _print = print;
        }

        public async Task<IEnumerable<ProductionInfo>> GetProductionDetailReport(int runID, int serviceCompanyID, int paperMediaID, int stationMediaID, int expeditionType, string expCode, bool hasColorPages, int envMaterialID, int plexType)
        {
            
            IEnumerable<ProductionInfo> productionReport = await _repository.ProductionReport.GetProductionDetailReport(runID,  serviceCompanyID, paperMediaID, stationMediaID,  expeditionType, expCode, hasColorPages, envMaterialID, plexType);
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
            ProductionReportViewModel viewmodel = new ProductionReportViewModel();
            viewmodel.ProductionReport = productionReport;
            bool existsToPrintFiles = false;
            bool printColor = false;
            bool printBlack = false;
            int plexFeature = 0;
            foreach (ProductionDetailInfo pdi in viewmodel.ProductionReport)
            {
                pdi.ProductionDetailReport = await _repository.ProductionReport.GetProductionDetailReport(runID, serviceCompanyID, pdi.PaperMediaID, pdi.StationMediaID, pdi.ExpeditionType, pdi.ExpCode, pdi.HasColorPages, pdi.EnvMaterialID, pdi.PlexType);
                foreach (ProductionInfo f in pdi.ProductionDetailReport)
                {
                    if (!f.FilePrintedFlag || !f.RegistDetailFilePrintedFlag)
                        existsToPrintFiles = true;
                    if (!string.IsNullOrEmpty(f.FilePrinterSpecs) && !f.FilePrintedFlag)
                    {
                        printColor = false;
                        printBlack = false;
                        plexFeature = 0;
                        _print.GetPrinterFeatures(f.FilePrinterSpecs, ref printColor, ref printBlack, ref plexFeature);
                        f.FilePrintColor = printColor;
                        f.FilePrintBlack = printBlack;
                        f.FilePlexType = plexFeature;
                    }
                    if (!string.IsNullOrEmpty(f.RegistDetailFilePrinterSpecs) && !f.RegistDetailFilePrintedFlag)
                    {
                        printColor = false;
                        printBlack = false;
                        plexFeature = 0;
                        _print.GetPrinterFeatures(f.RegistDetailFilePrinterSpecs, ref printColor, ref printBlack, ref plexFeature);
                        f.RegistDetailFilePrintColor = printColor;
                        f.RegistDetailFilePrintBlack = printBlack;
                        f.RegistDetailFilePlexType = plexFeature;
                    }
                }
            }
            if (existsToPrintFiles)
            {
                IEnumerable<ResourceInfo> result = await _repository.RegistJob.GetResources("PRINTER", profileList, "", false);
                if (result != null)
                {
                    List<PrinterInfo> Printers = new List<PrinterInfo>();
                    foreach (ResourceInfo r in result)
                    {
                        PrinterInfo p = new PrinterInfo();
                        p.ResValue = r.ResValue;
                        p.MatchFilter = r.MatchFilter;
                        p.ResID = r.ResID;
                        p.Description = r.Description;

                        printColor = false;
                        printBlack = false;
                        plexFeature = 0;
                        _print.GetPrinterFeatures(p.ResValue, ref printColor, ref printBlack, ref plexFeature);
                        p.PrintColor = printColor;
                        p.PrintBlack = printBlack;
                        p.PlexFeature = plexFeature;

                        Printers.Add(p);
                    }
                    viewmodel.Printers = Printers;
                }
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
