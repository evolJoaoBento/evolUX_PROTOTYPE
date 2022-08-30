﻿

using evolUX.API.Areas.Finishing.Models;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IProductionReportRepository
    {
        public Task<IEnumerable<ProductionRunInfo>> GetProductionRunReport(DataTable ServiceCompanyList);
        public Task<IEnumerable<ProductionInfo>> GetProductionDetailReport(int runID, int serviceCompanyID, int paperMediaID, int stationMediaID, int expeditionType, string expCode, bool hasColorPages);
        public Task<IEnumerable<ProductionDetailInfo>> GetProductionReport(int runID, int serviceCompanyID);
    }
}
