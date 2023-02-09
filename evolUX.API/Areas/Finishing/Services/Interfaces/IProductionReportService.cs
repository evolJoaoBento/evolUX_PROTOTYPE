﻿using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IProductionReportService
    {
        public Task<IEnumerable<ProductionRunInfo>> GetProductionRunReport(int ServiceCompanyID);
        public Task<ProductionReportViewModel> GetProductionReport(IEnumerable<int> profileList, DataTable runIDList, int serviceCompanyID, bool filterOnlyPrint);
    }
}
