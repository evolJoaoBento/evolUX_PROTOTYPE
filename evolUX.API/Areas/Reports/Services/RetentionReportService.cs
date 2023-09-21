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
