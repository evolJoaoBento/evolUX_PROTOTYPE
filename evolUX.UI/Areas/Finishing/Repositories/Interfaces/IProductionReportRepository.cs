﻿using Flurl.Http;
using Shared.Models.Areas.Finishing;
using Shared.ViewModels.Areas.Finishing;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Repositories.Interfaces
{
    public interface IProductionReportRepository
    {
        public Task<ProductionRunReportViewModel> GetProductionRunReport(int ServiceCompanyID);
        public Task<ProductionReportViewModel> GetProductionReport(string profileList, List<int> runIDList, int serviceCompanyID, bool filterOnlyPrint);
        public Task<IEnumerable<ProductionDetailInfo>> GetProductionReportFilters(string profileList, List<int> runIDList, int serviceCompanyID, bool filterOnlyPrint);
    }
}