using Dapper;
using evolUX.API.Areas.Finishing.Models;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Data.Repositories
{
    public class ProductionReportRepository : IProductionReportRepository
    {
        private readonly DapperContext _context;
        public ProductionReportRepository(DapperContext context)
        {
            _context = context;
        }

        //FOR REFERENCE https://www.faqcode4u.com/faq/530844/dapper-mapping-dynamic-pivot-columns-from-stored-procedure
        //              https://stackoverflow.com/questions/8229927/looping-through-each-element-in-a-datarow
        public async Task<IEnumerable<ProductionInfo>> GetProductionDetailReport(int runID, int serviceCompanyID, int paperMediaID,
                    int stationMediaID, int expeditionType, string expCode, bool hasColorPages)
        {
            var lookup = new Dictionary<int, ProductionInfo>();

            string sql = @"EXEC RP_UX_PRODUCTION_REPORT     @ServiceCompanyID, 
                                                            @RunID, 
                                                            @PaperMediaID, 
                                                            @StationMediaID, 
                                                            @ExpeditionMediaID, 
                                                            @ExpCode, 
                                                            @HasColorPages ";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            parameters.Add("RunID", runID, DbType.Int64);
            parameters.Add("PaperMediaID", paperMediaID, DbType.Int64);
            parameters.Add("StationMediaID", stationMediaID, DbType.Int64);
            parameters.Add("ExpeditionMediaID", expeditionType, DbType.Int64);
            parameters.Add("ExpCode", expCode, DbType.Int64);
            parameters.Add("HasColorPages", hasColorPages, DbType.Binary);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                List<ProductionInfo> productReport = new List<ProductionInfo>();

                connection.Open();
                var obs = await connection.QueryAsync(sql, parameters,
                                    commandType: CommandType.StoredProcedure);
                var dt = _context.ToDataTable(obs);

                foreach (DataRow r in dt.Rows)
                {
                    ProductionInfo productionInfo = new ProductionInfo();
                    productionInfo.RunID = (int)r["RunID"];
                    productionInfo.FileID = (int)r["FileID"];
                    productionInfo.FilePath = (string)r["FilePath"];
                    productionInfo.FileName = (string)r["FileName"];
                    productionInfo.ShortFileName = (string)r["ShortFileName"];
                    productionInfo.FilePrinterSpecs = (string)r["FilePrinterSpecs"];
                    productionInfo.RegistShortFileName = (string)r["RegistShortFileName"];
                    productionInfo.RegistFilePrinterSpecs = (string)r["RegistFilePrinterSpecs"];
                    productionInfo.ServiceTaskCode = (int)r["ServiceTaskCode"];
                    productionInfo.PrinterOperator = (string)r["PrinterOperator"];
                    productionInfo.Printer = (string)r["Printer"];
                    productionInfo.PlexCode = (int)r["PlexCode"];
                    productionInfo.TotalPrint = (int)r["TotalPrint"];
                    productionInfo.StartSeqNum = (int)r["StartSeqNum"];
                    productionInfo.EndSeqNum = (int)r["EndSeqNum"];
                    productionInfo.FullFillMaterialRef = (string)r["FullFillMaterialRef"];
                    productionInfo.TotalPostObjs = (int)r["TotalPostObjs"];
                    productionInfo.ExpLevel = (int)r["ExpLevel"];
                    productionInfo.ExpCompanyCode = (int)r["ExpCompanyCode"];
                    productionInfo.ExpCenterCodeDesc = (string)r["ExpCenterCodeDesc"];
                    productionInfo.ExpeditionZone = (string)r["ExpeditionZone"];

                    for (int i = 21; i < dt.Columns.Count; i++)
                    {
                        string[] strings = dt.Columns[i].ColumnName.Split("|");
                        if (strings[0] == "Paper")
                        {
                            productionInfo.PaperTotals = productionInfo.PaperTotals ?? new Dictionary<string, int>();
                            productionInfo.PaperTotals.Add(strings[1], value: (int)r.ItemArray[i]);
                        }
                        else if (strings[0] == "Station")
                        {
                            productionInfo.StationTotals = productionInfo.StationTotals ?? new Dictionary<string, int>();
                            productionInfo.StationTotals.Add(strings[1], value: (int)r.ItemArray[i]);
                        }

                    }

                    productReport.Add(productionInfo);
                }
                return productReport;
            }
        }

        public async Task<IEnumerable<ProductionDetailInfo>> GetProductionReport(int runID, int serviceCompanyID)
        {
            string sql = @"EXEC RP_UX_PRODUCTION_DETAIL_REPORT      @ServiceCompanyID
                                                                    @RunID";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            parameters.Add("RunID", runID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ProductionDetailInfo> envelopeMediaList = await connection.QueryAsync<ProductionDetailInfo>(sql, parameters,
                            commandType: CommandType.StoredProcedure);
                return envelopeMediaList;
            }
        }


        //FOR REFERENCE https://stackoverflow.com/questions/33087629/dapper-dynamic-parameters-with-table-valued-parameters
        public async Task<IEnumerable<ProductionRunInfo>> GetProductionRunReport(DataTable ServiceCompanyList)
        {
            
            string sql = @"EXEC RP_UX_PRODUCTION_RUN_REPORT     @ServiceCompanyList";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyList", ServiceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ProductionRunInfo> productionRunReport = await connection.QueryAsync<ProductionRunInfo, ProductionRunDetail,
                            ProductionRunDetail, ProductionRunDetail, ProductionRunDetail, ProductionRunDetail, ProductionRunDetail,
                            ProductionRunInfo>(sql, (r, d1, d2, d3, d4, d5, d6) =>
                {
                    ProductionRunInfo runInfo = r;
                    runInfo.Processed = d1;
                    runInfo.ToPrint = d2;
                    runInfo.S2Printer = d3;
                    runInfo.Printed = d4;
                    runInfo.FullFill = d5;
                    runInfo.Expedition = d6;
                    return runInfo;
                }, parameters, commandType: CommandType.StoredProcedure, splitOn: "Total");
                return productionRunReport;
            }
        }

    }
}
