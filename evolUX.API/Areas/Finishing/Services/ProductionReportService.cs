using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using System.Data;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using Shared.Models.Areas.Core;
using System.Xml;
using Shared.Models.General;
using evolUX.API.Models;

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
            int colorFeature = 0;
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
                        colorFeature = 0;
                        plexFeature = 0;
                        _print.GetPrinterFeatures(f.FilePrinterSpecs, ref colorFeature, ref plexFeature);
                        f.FileColor = colorFeature;
                        f.FilePlexType = plexFeature;
                    }
                    if (!string.IsNullOrEmpty(f.RegistDetailFilePrinterSpecs) && !f.RegistDetailFilePrintedFlag)
                    {
                        colorFeature = 0;
                        plexFeature = 0;
                        _print.GetPrinterFeatures(f.RegistDetailFilePrinterSpecs, ref colorFeature, ref plexFeature);
                        f.RegistDetailFileColor = colorFeature;
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

                        colorFeature = 0;
                        plexFeature = 0;
                        _print.GetPrinterFeatures(p.ResValue, ref colorFeature, ref plexFeature);
                        p.ColorFeature = colorFeature;
                        p.PlexFeature = plexFeature;

                        Printers.Add(p);
                    }
                    viewmodel.Printers = Printers;
                }
            }

            return viewmodel;
        }

        public async Task<ProductionReportPrinterViewModel> GetProductionPrinterReport(IEnumerable<int> profileList, int runID, int serviceCompanyID)
        {
            IEnumerable<ProductionDetailInfo> productionReport = await _repository.ProductionReport.GetProductionReport(runID, serviceCompanyID);
            if (productionReport == null)
            {

            }
            ProductionReportPrinterViewModel viewmodel = new ProductionReportPrinterViewModel();

            int lastServiceCompanyID = 0;
            int lastExpCompanyID = 0;
            int lastExpeditionType = 0;
            string lastServiceTaskCode = string.Empty;
            string lastFullFillMaterialCode = string.Empty;

            List<ProdServiceCompanyElement> ProdDetailReport = new List<ProdServiceCompanyElement>();
            ProdServiceCompanyElement ServiceCompany = new ProdServiceCompanyElement();
            ProdExpeditionElement ExpeditionList = new ProdExpeditionElement();
            ProdServiceElement ServiceList = new ProdServiceElement();
            ProdFullFillElement FullFillList = new ProdFullFillElement();

            foreach (ProductionDetailInfo pdi in productionReport)
            {
                if (pdi.ServiceCompanyID != lastServiceCompanyID)
                {
                    ServiceCompany = new ProdServiceCompanyElement();
                    ProdDetailReport.Add(ServiceCompany);
                    ServiceCompany.ServiceCompanyID = pdi.ServiceCompanyID;
                    ServiceCompany.ServiceCompanyCode = pdi.ServiceCompanyCode;
                    ServiceCompany.ServiceCompanyName = pdi.ServiceCompanyName;
                    ServiceCompany.ExpeditionList = new List<ProdExpeditionElement>();
                    lastServiceCompanyID = pdi.ServiceCompanyID;
                    lastExpCompanyID = 0;
                    lastExpeditionType = 0;
                    lastServiceTaskCode = string.Empty;
                    lastFullFillMaterialCode = string.Empty;
                }
                if (pdi.ExpCompanyID != lastExpCompanyID || pdi.ExpeditionType != lastExpeditionType)
                {
                    ExpeditionList = new ProdExpeditionElement();
                    ServiceCompany.ExpeditionList.Add(ExpeditionList);
                    ExpeditionList.ExpCompanyID = pdi.ExpCompanyID;
                    ExpeditionList.ExpCompanyCode = pdi.ExpCompanyCode;
                    ExpeditionList.ExpCompanyName = pdi.ExpCompanyName;
                    ExpeditionList.ExpeditionType = pdi.ExpeditionType;
                    ExpeditionList.ExpeditionTypeDesc = pdi.ExpeditionTypeDesc;
                    ExpeditionList.ServiceList = new List<ProdServiceElement>();
                    lastExpCompanyID = pdi.ExpCompanyID;
                    lastExpeditionType = pdi.ExpeditionType;
                    lastServiceTaskCode = string.Empty;
                    lastFullFillMaterialCode = string.Empty;
                }
                if (pdi.ServiceTaskCode != lastServiceTaskCode)
                {
                    ServiceList = new ProdServiceElement();
                    ExpeditionList.ServiceList.Add(ServiceList);
                    ServiceList.ServiceTaskCode = pdi.ServiceTaskCode;
                    ServiceList.ServiceTaskDec = pdi.ServiceTaskDesc;
                    ServiceList.FullFillList = new List<ProdFullFillElement>();
                    lastServiceTaskCode = pdi.ServiceTaskCode;
                    lastFullFillMaterialCode = string.Empty;
                }
                if (pdi.FullFillMaterialCode != lastFullFillMaterialCode)
                {
                    FullFillList = new ProdFullFillElement();
                    ServiceList.FullFillList.Add(FullFillList);
                    FullFillList.FullFillMaterialCode = pdi.FullFillMaterialCode;
                    FullFillList.FullFillCapacity = pdi.FullFillCapacity;
                    lastFullFillMaterialCode = pdi.FullFillMaterialCode;
                }
                IEnumerable<ProdFileInfo> FileList = await _repository.ProductionReport.GetProductionDetailPrinterReport(_print, runID, serviceCompanyID, pdi.PaperMediaID, pdi.StationMediaID, pdi.ExpeditionType, pdi.ExpCode, pdi.HasColorPages, pdi.EnvMaterialID, pdi.PlexType);
                FullFillList.FileList = FileList.ToList();
            }
            viewmodel.ProductionReport = ProdDetailReport;
 
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

                    int colorFeature = 0;
                    int plexFeature = 0;
                    _print.GetPrinterFeatures(p.ResValue, ref colorFeature, ref plexFeature);
                    p.ColorFeature = colorFeature;
                    p.PlexFeature = plexFeature;

                    Printers.Add(p);
                }
                viewmodel.Printers = Printers;
            }

            return viewmodel;
        }

        public async Task<IEnumerable<ProductionRunInfo>> GetProductionRunReport(int ServiceCompanyID)
        {
            IEnumerable<ProductionRunInfo> productionRunReport = await _repository.ProductionReport.GetProductionRunReport(ServiceCompanyID);
            if (productionRunReport == null)
            {

            }

            return productionRunReport;
        }
    }
}
