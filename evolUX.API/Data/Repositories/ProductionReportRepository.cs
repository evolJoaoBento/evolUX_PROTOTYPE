using Dapper;
using Shared.Models.Areas.Finishing;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using System.Data;
using System.Data.SqlClient;
using evolUX.API.Extensions;

namespace evolUX.API.Data.Repositories
{
    public class ProductionReportRepository : IProductionReportRepository
    {
        private readonly DapperContext _context;
        public ProductionReportRepository(DapperContext context)
        {
            _context = context;
        }

        
        //SUM TOTAL AND DYNAMICS
        //FOR REFERENCE https://www.faqcode4u.com/faq/530844/dapper-mapping-dynamic-pivot-columns-from-stored-procedure
        //              https://stackoverflow.com/questions/8229927/looping-through-each-element-in-a-datarow
        public async Task<IEnumerable<ProductionInfo>> GetProductionDetailReport(int runID, int serviceCompanyID, int paperMediaID,
                    int stationMediaID, int expeditionType, string expCode, bool hasColorPages)
        {
            var lookup = new Dictionary<int, ProductionInfo>();

            string sql = @"RP_UX_PRODUCTION_REPORT";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            parameters.Add("RunID", runID, DbType.Int64);
            parameters.Add("PaperMediaID", paperMediaID, DbType.Int64);
            parameters.Add("StationMediaID", stationMediaID, DbType.Int64);
            parameters.Add("ExpeditionType", expeditionType, DbType.Int64);
            parameters.Add("ExpCode", expCode, DbType.String);
            parameters.Add("HasColorPages", hasColorPages, DbType.Boolean);

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
                    productionInfo.RegistDetailFileName = (string)r["RegistDetailFileName"];
                    productionInfo.RegistDetailShortFileName = (string)r["RegistDetailShortFileName"];
                    productionInfo.RegistDetailFilePrinterSpecs = (string)r["RegistDetailFilePrinterSpecs"];
                    productionInfo.RegistDetailFilePrintedFlag = (bool)r["RegistDetailFilePrintedFlag"];
                    productionInfo.FilePrintedFlag = (bool)r["FilePrintedFlag"];
                    productionInfo.ServiceTaskCode = (string)r["ServiceTaskCode"];
                    productionInfo.PrinterOperator = r["PrinterOperator"].ConvertFromDBVal<string>();
                    productionInfo.Printer = r["Printer"].ConvertFromDBVal<string>();
                    productionInfo.PlexCode = (string)r["PlexCode"];
                    productionInfo.TotalPrint = (int)r["TotalPrint"];
                    productionInfo.StartSeqNum = (int)r["StartSeqNum"];
                    productionInfo.EndSeqNum = (int)r["EndSeqNum"];
                    productionInfo.FullFillMaterialRef = (string)r["FullFillMaterialRef"];
                    productionInfo.FullFillMaterialCode = (string)r["FullFillMaterialCode"];
                    productionInfo.TotalPostObjs = (int)r["TotalPostObjs"];
                    productionInfo.ExpLevel = (int)r["ExpLevel"];
                    productionInfo.ExpCompanyCode = (string)r["ExpCompanyCode"];
                    productionInfo.ExpCenterCode = (string)r["ExpCenterCode"];//expCenterCode
                    productionInfo.ExpeditionZone = (string)r["ExpZone"];

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
            string sql = @"RP_UX_PRODUCTION_DETAIL_REPORT";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            parameters.Add("RunID", runID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                //pass all servicecompany runid

                IEnumerable<ProductionDetailInfo> envelopeMediaList = await connection.QueryAsync<ProductionDetailInfo>(sql, parameters,
                            commandType: CommandType.StoredProcedure);
                return envelopeMediaList;
            }
        }


        //FOR REFERENCE https://stackoverflow.com/questions/33087629/dapper-dynamic-parameters-with-table-valued-parameters
        public async Task<IEnumerable<ProductionRunInfo>> GetProductionRunReport(DataTable ServiceCompanyList)
        {
            
            string sql = @"RP_UX_PRODUCTION_RUN_REPORT";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyList", ServiceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                
                //DataTable dt = new DataTable();
                //SqlConnection conn = new SqlConnection(connection.ConnectionString);
                //SqlCommand cmd = new SqlCommand(sql, conn); 
                //cmd.Parameters.Add(new SqlParameter("@ServiceCompanyList", ServiceCompanyList));
                //cmd.CommandType = CommandType.StoredProcedure;

                //conn.Open();

                //// create data adapter
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //// this will query your database and return the result to your datatable
                //da.Fill(dt);
                //conn.Close();
                //da.Dispose();

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
                }, parameters, commandType: CommandType.StoredProcedure, splitOn: "TotalProcessed, TotalToPrint, TotalS2Printer, TotalPrinted, TotalFullFill, TotalExpedition");
                return productionRunReport;
            }

        }
       
    }
}
