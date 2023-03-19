using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using System.Data;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using Shared.Models.Areas.Core;
using System.Xml;
using Shared.Models.General;
using evolUX.API.Models;
using System.Collections.Generic;

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


        public async Task<ProductionReportViewModel> GetProductionReport(IEnumerable<int> profileList, DataTable runIDList, int serviceCompanyID, bool filterOnlyPrint)
        {
            IEnumerable<ProductionDetailInfo> productionReport = await _repository.ProductionReport.GetProductionReport(runIDList, serviceCompanyID);
            ProductionReportViewModel viewmodel = new ProductionReportViewModel();
            if (productionReport == null || productionReport.Count() == 0)
                return viewmodel;

            viewmodel.ServiceCompanyID = serviceCompanyID;
            viewmodel.ServiceCompanyName = productionReport.First().ServiceCompanyName;
            viewmodel.ServiceCompanyCode = productionReport.First().ServiceCompanyCode;

            int lastExpCompanyID = 0;
            int lastExpeditionType = 0;
            string lastServiceTaskCode = string.Empty;
            int lastPaperMediaID = 0;
            int lastStationMediaID = -1;
            int lastPlexType = 0;

            List<ProdExpeditionElement> ProdDetailReport = new List<ProdExpeditionElement>();
            ProdExpeditionElement ExpeditionList = new ProdExpeditionElement();
            ProdServiceElement ServiceList = new ProdServiceElement();
            ProdMaterialElement MaterialMediaList = new ProdMaterialElement();

            foreach (ProductionDetailInfo pdi in productionReport)
            {
                IEnumerable<ProdFileInfo> FileList = await _repository.ProductionReport.GetProductionDetailReport(_print, runIDList, serviceCompanyID, pdi.PaperMediaID, pdi.StationMediaID, pdi.ExpeditionType, pdi.ExpCompanyID, pdi.ServiceTaskID, pdi.HasColorPages, pdi.PlexType, filterOnlyPrint);

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
                        lastPlexType = 0;
                    }
                    if (pdi.ServiceTaskCode != lastServiceTaskCode)
                    {
                        ServiceList = new ProdServiceElement();
                        ExpeditionList.ServiceList.Add(ServiceList);
                        ServiceList.ServiceTaskCode = pdi.ServiceTaskCode;
                        ServiceList.ServiceTaskID = pdi.ServiceTaskID;
                        ServiceList.ServiceTaskDesc = pdi.ServiceTaskDesc;
                        ServiceList.MediaMaterialList = new List<ProdMaterialElement>();
                        lastServiceTaskCode = pdi.ServiceTaskCode;
                        lastPaperMediaID = 0;
                        lastStationMediaID = -1;
                        lastPlexType = 0;
                    }
                    if (pdi.PaperMediaID != lastPaperMediaID || pdi.StationMediaID != lastStationMediaID || pdi.PlexType != lastPlexType)
                    {
                        MaterialMediaList = new ProdMaterialElement();
                        ServiceList.MediaMaterialList.Add(MaterialMediaList);
                        MaterialMediaList.PaperMediaID = pdi.PaperMediaID;
                        MaterialMediaList.PaperMaterialList = pdi.PaperMaterialList;
                        MaterialMediaList.StationMediaID = pdi.StationMediaID;
                        MaterialMediaList.StationMaterialList = pdi.StationMaterialList;
                        MaterialMediaList.PlexType = pdi.PlexType;
                        MaterialMediaList.PlexCode = pdi.PlexCode;
                        lastPaperMediaID = pdi.PaperMediaID;
                        lastStationMediaID = pdi.StationMediaID;
                        lastPlexType = pdi.PlexType;
                    }
                    MaterialMediaList.FileList = FileList.ToList();
                }
            }
            viewmodel.ProductionReport = ProdDetailReport;

            if (filterOnlyPrint)
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

                        int colorFeature = 0;
                        int plexFeature = 0;
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

        public async Task<IEnumerable<ProductionDetailInfo>> GetProductionReportFilters(IEnumerable<int> profileList, DataTable runIDList, int serviceCompanyID, bool filterOnlyPrint)
        {
            IEnumerable<ProductionDetailInfo> productionReport = await _repository.ProductionReport.GetProductionReport(runIDList, serviceCompanyID);
            return productionReport;
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
