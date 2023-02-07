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

        public async Task<IEnumerable<ProductionInfo>> GetProductionDetailReport(int runID, int serviceCompanyID, int paperMediaID, int stationMediaID, int expeditionType, string expCode, bool hasColorPages, int plexType)
        {
            
            IEnumerable<ProductionInfo> productionReport = await _repository.ProductionReport.GetProductionDetailReport(runID,  serviceCompanyID, paperMediaID, stationMediaID,  expeditionType, expCode, hasColorPages, plexType);
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
                pdi.ProductionDetailReport = await _repository.ProductionReport.GetProductionDetailReport(runID, serviceCompanyID, pdi.PaperMediaID, pdi.StationMediaID, pdi.ExpeditionType, pdi.ExpCode, pdi.HasColorPages, pdi.PlexType);
                foreach (ProductionInfo f in pdi.ProductionDetailReport)
                {
                    if (!f.FilePrintedFlag || !f.RegistDetailFilePrintedFlag)
                        existsToPrintFiles = true;
                    f.FileColor = 3;
                    f.FilePlexType = 3;
                    if (!string.IsNullOrEmpty(f.FilePrinterSpecs) && !f.FilePrintedFlag)
                    {
                        colorFeature = 0;
                        plexFeature = 0;
                        _print.GetPrinterFeatures(f.FilePrinterSpecs, ref colorFeature, ref plexFeature);
                        f.FileColor = colorFeature;
                        f.FilePlexType = plexFeature;
                    }
                    f.RegistDetailFileColor = 3;
                    f.RegistDetailFilePlexType = 3;
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

            int lastExpCompanyID = 0;
            int lastExpeditionType = 0;
            string lastServiceTaskCode = string.Empty;
            int lastPaperMediaID = 0;
            int lastStationMediaID = -1;

            List<ProdExpeditionElement> ProdDetailReport = new List<ProdExpeditionElement>();
            ProdExpeditionElement ExpeditionList = new ProdExpeditionElement();
            ProdServiceElement ServiceList = new ProdServiceElement();
            ProdMaterialElement MaterialMediaList = new ProdMaterialElement();

            foreach (ProductionDetailInfo pdi in productionReport)
            {
                IEnumerable<ProdFileInfo> FileList = await _repository.ProductionReport.GetProductionDetailPrinterReport(_print, runID, serviceCompanyID, pdi.PaperMediaID, pdi.StationMediaID, pdi.ExpeditionType, pdi.ExpCompanyID, pdi.ServiceTaskID, pdi.HasColorPages, pdi.PlexType);

                if (FileList != null && FileList.Count() > 0)
                {
                    if (pdi.ExpCompanyID != lastExpCompanyID || pdi.ExpeditionType != lastExpeditionType)
                    {
                        ExpeditionList = new ProdExpeditionElement();
                        ProdDetailReport.Add(ExpeditionList);
                        ExpeditionList.ExpCompanyID = pdi.ExpCompanyID;
                        ExpeditionList.ExpCompanyCode = pdi.ExpCompanyCode;
                        ExpeditionList.ExpCompanyName = pdi.ExpCompanyName;
                        ExpeditionList.ExpeditionType = pdi.ExpeditionType;
                        ExpeditionList.ExpeditionTypeDesc = pdi.ExpeditionTypeDesc;
                        ExpeditionList.ServiceList = new List<ProdServiceElement>();
                        lastExpCompanyID = pdi.ExpCompanyID;
                        lastExpeditionType = pdi.ExpeditionType;
                        lastServiceTaskCode = string.Empty;
                        lastPaperMediaID = 0;
                        lastStationMediaID = -1;
                    }
                    if (pdi.ServiceTaskCode != lastServiceTaskCode)
                    {
                        ServiceList = new ProdServiceElement();
                        ExpeditionList.ServiceList.Add(ServiceList);
                        ServiceList.ServiceTaskCode = pdi.ServiceTaskCode;
                        ServiceList.ServiceTaskDec = pdi.ServiceTaskDesc;
                        ServiceList.MediaMaterialList = new List<ProdMaterialElement>();
                        lastServiceTaskCode = pdi.ServiceTaskCode;
                        lastPaperMediaID = 0;
                        lastStationMediaID = -1;
                    }
                    if (pdi.PaperMediaID != lastPaperMediaID || pdi.StationMediaID != lastStationMediaID)
                    {
                        MaterialMediaList = new ProdMaterialElement();
                        ServiceList.MediaMaterialList.Add(MaterialMediaList);
                        MaterialMediaList.PaperMediaID = pdi.PaperMediaID;
                        MaterialMediaList.PaperMaterialList = pdi.PaperMaterialList;
                        MaterialMediaList.StationMediaID = pdi.StationMediaID;
                        MaterialMediaList.StationMaterialList = pdi.StationMaterialList;
                        lastPaperMediaID = pdi.PaperMediaID;
                        lastStationMediaID = pdi.StationMediaID;
                    }
                    MaterialMediaList.FileList = FileList.ToList();
                }
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
