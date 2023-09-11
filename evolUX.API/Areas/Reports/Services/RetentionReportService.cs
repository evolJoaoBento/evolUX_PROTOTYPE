using evolUX.API.Areas.Reports.Services.Interfaces;
using Shared.ViewModels.Areas.Reports;
using Shared.Models.Areas.Reports;
using System.Data;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using Shared.Models.Areas.Core;
using System.Xml;
using Shared.Models.General;
using evolUX.API.Models;
using System.Collections.Generic;
using Shared.ViewModels.Areas.Finishing;

namespace evolUX.API.Areas.Reports.Services
{
    public class RetentionReportService : IRetentionReportService
    {
        private readonly IWrapperRepository _repository;
        public RetentionReportService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<RetentionRunReportViewModel> GetRetentionRunReport(int BusinessAreaID, int RefDate)
        {
            RetentionRunReportViewModel viewmodel = new RetentionRunReportViewModel();
            viewmodel.RetentionRunReport = await _repository.RetentionReport.GetRetentionRunReport(BusinessAreaID, RefDate);
            viewmodel.RefDate = RefDate;
            if (viewmodel.RetentionRunReport == null)
            {
                throw new Exception();//MAKE BETTER EXCEPTION
            }

            return viewmodel;
        }

        public async Task<RetentionReportViewModel> GetRetentionReport(DataTable runIDList, int businessAreaID)
            {
                RetentionReportViewModel viewmodel = new RetentionReportViewModel();
                viewmodel.RetentionReport = await _repository.RetentionReport.GetRetentionReport(runIDList, businessAreaID);
                viewmodel.BusinessAreaID = businessAreaID;
                if (viewmodel.RetentionReport == null)
                {
                    throw new Exception();//MAKE BETTER EXCEPTION
                }

                return viewmodel;

            //IEnumerable<RetentionRunInfo> retentionReport = await _repository.RetentionReport.GetRetentionReport(runIDList, businessAreaID);
            //RetentionReportViewModel viewmodel = new RetentionReportViewModel();
            //if (retentionReport == null || retentionReport.Count() == 0)
            //    return viewmodel;

            //viewmodel.BusinessAreaID = businessAreaID;

            //int lastExpCompanyID = 0;
            //int lastExpeditionType = 0;
            //string lastServiceTaskCode = string.Empty;
            //int lastPaperMediaID = 0;
            //int lastStationMediaID = -1;
            //int lastPlexType = 0;

            //List<ProdExpeditionElement> ProdDetailReport = new List<ProdExpeditionElement>();
            //ProdExpeditionElement ExpeditionList = new ProdExpeditionElement();
            //ProdServiceElement ServiceList = new ProdServiceElement();
            //ProdMaterialElement MaterialMediaList = new ProdMaterialElement();

            //foreach (RetentionDetailInfo pdi in retentionReport)
            //{
            //    IEnumerable<ProdFileInfo> FileList = await _repository.RetentionReport.GetRetentionDetailReport(runIDList, businessAreaID);

            //    if (FileList != null && FileList.Count() > 0)
            //    {
            //        if (pdi.ExpCompanyID != lastExpCompanyID || pdi.ExpeditionType != lastExpeditionType)
            //        {
            //            ExpeditionList = new ProdExpeditionElement();
            //            ProdDetailReport.Add(ExpeditionList);
            //            ExpeditionList.ExpCompanyID = pdi.ExpCompanyID;
            //            ExpeditionList.ExpCompanyCode = pdi.ExpCompanyCode;
            //            ExpeditionList.ExpCompanyName = pdi.ExpCompanyName;
            //            ExpeditionList.ExpeditionType = pdi.ExpeditionType;
            //            ExpeditionList.ExpeditionTypeDesc = pdi.ExpeditionTypeDesc;
            //            ExpeditionList.ServiceList = new List<ProdServiceElement>();
            //            lastExpCompanyID = pdi.ExpCompanyID;
            //            lastExpeditionType = pdi.ExpeditionType;
            //            lastServiceTaskCode = string.Empty;
            //            lastPaperMediaID = 0;
            //            lastStationMediaID = -1;
            //            lastPlexType = 0;
            //        }
            //        if (pdi.ServiceTaskCode != lastServiceTaskCode)
            //        {
            //            ServiceList = new ProdServiceElement();
            //            ExpeditionList.ServiceList.Add(ServiceList);
            //            ServiceList.ServiceTaskCode = pdi.ServiceTaskCode;
            //            ServiceList.ServiceTaskID = pdi.ServiceTaskID;
            //            ServiceList.ServiceTaskDesc = pdi.ServiceTaskDesc;
            //            ServiceList.MediaMaterialList = new List<ProdMaterialElement>();
            //            lastServiceTaskCode = pdi.ServiceTaskCode;
            //            lastPaperMediaID = 0;
            //            lastStationMediaID = -1;
            //            lastPlexType = 0;
            //        }
            //        if (pdi.PaperMediaID != lastPaperMediaID || pdi.StationMediaID != lastStationMediaID || pdi.PlexType != lastPlexType)
            //        {
            //            MaterialMediaList = new ProdMaterialElement();
            //            ServiceList.MediaMaterialList.Add(MaterialMediaList);
            //            MaterialMediaList.PaperMediaID = pdi.PaperMediaID;
            //            MaterialMediaList.PaperMaterialList = pdi.PaperMaterialList;
            //            MaterialMediaList.StationMediaID = pdi.StationMediaID;
            //            MaterialMediaList.StationMaterialList = pdi.StationMaterialList;
            //            MaterialMediaList.PlexType = pdi.PlexType;
            //            MaterialMediaList.PlexCode = pdi.PlexCode;
            //            lastPaperMediaID = pdi.PaperMediaID;
            //            lastStationMediaID = pdi.StationMediaID;
            //            lastPlexType = pdi.PlexType;
            //        }
            //        MaterialMediaList.FileList = FileList.ToList();
            //    }
            //}
            return null;
        }

        public async Task<RetentionInfoReportViewModel> GetRetentionInfoReport(int RunID, int FileID, int SetID, int DocID)
        {
            RetentionInfoReportViewModel viewmodel = new RetentionInfoReportViewModel();
            viewmodel.RetentionInfo = await _repository.RetentionReport.GetRetentionInfoReport(RunID, FileID, SetID, DocID);
            viewmodel.RunID = RunID;
            if (viewmodel.RetentionInfo == null)
            {
                throw new Exception();//MAKE BETTER EXCEPTION
            }

            return viewmodel;
        }
    }
}
