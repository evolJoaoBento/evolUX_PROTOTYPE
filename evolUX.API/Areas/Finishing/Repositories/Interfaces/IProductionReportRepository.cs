﻿using evolUX.API.Areas.Finishing.Services.Interfaces;
using Shared.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Finishing.Repositories.Interfaces
{
    public interface IProductionReportRepository
    {
        public Task LogSentToPrinter(int runID, int fileID); 
        public Task<IEnumerable<ProductionRunInfo>> GetProductionRunReport(int ServiceCompanyID);
        public Task<IEnumerable<ProductionInfo>> GetProductionDetailReport(int runID, int serviceCompanyID, int paperMediaID, int stationMediaID, int expeditionType, string expCode, bool hasColorPages, int plexType);
        public Task<IEnumerable<ProdFileInfo>> GetProductionDetailPrinterReport(IPrintService print, int runID, int serviceCompanyID, int paperMediaID, int stationMediaID, int expeditionType, int expCompanyID, int serviceTaskID, bool hasColorPages, int plexType);
        public Task<IEnumerable<ProductionDetailInfo>> GetProductionReport(int runID, int serviceCompanyID);
        public Task<string> GetServiceCompanyCode(int serviceCompanyID);

    }
}
