using Dapper;
using Shared.Models.Areas.Finishing;
using evolUX.API.Data.Context;
using System.Data;
using System.Data.SqlClient;
using evolUX.API.Extensions;
using evolUX.API.Areas.Finishing.Repositories.Interfaces;
using evolUX.API.Models;
using System.Drawing;
using evolUX.API.Areas.Finishing.Services.Interfaces;

namespace evolUX.API.Areas.Finishing.Repositories
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
        public async Task LogSentToPrinter(int runID, int fileID)
        {
            string sql = @"RT_INSERT_INTO_FILE_LOG";
            var parameters = new DynamicParameters();
            parameters.Add("RunID", runID, DbType.Int64);
            parameters.Add("FileID", fileID, DbType.Int64);
            parameters.Add("RunStateName", "SEND2PRINTER", DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                //pass all servicecompany runid

                await connection.QueryAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<ProductionInfo>> GetProductionDetailReport(int runID, int serviceCompanyID, int paperMediaID,
                    int stationMediaID, int expeditionType, string expCode, bool hasColorPages, int envMaterialID, int plexType)
        {
            var lookup = new Dictionary<int, ProductionInfo>();

            string sql = @"RP_UX_PRODUCTION_REPORT";
            var parameters = new DynamicParameters();
            DataTable RunIDList = new DataTable();
            RunIDList.Columns.Add("ID", typeof(int));
            RunIDList.Rows.Add(runID);
            parameters.Add("RunIDList", RunIDList.AsTableValuedParameter("IDlist"));

            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            parameters.Add("PaperMediaID", paperMediaID, DbType.Int64);
            parameters.Add("StationMediaID", stationMediaID, DbType.Int64);
            parameters.Add("ExpeditionType", expeditionType, DbType.Int64);
            parameters.Add("ExpCode", expCode, DbType.String);
            parameters.Add("HasColorPages", hasColorPages, DbType.Boolean);
            parameters.Add("EnvMaterialID", envMaterialID, DbType.Int64);
            parameters.Add("PlexType", plexType, DbType.Int64);

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
                    productionInfo.EnvMaterialRef = (string)r["EnvMaterialRef"];
                    productionInfo.FullFillMaterialCode = (string)r["FullFillMaterialCode"];
                    productionInfo.TotalPostObjs = (int)r["TotalPostObjs"];
                    productionInfo.ExpLevel = (int)r["ExpLevel"];
                    productionInfo.ExpCompanyCode = (string)r["ExpCompanyCode"];
                    productionInfo.ExpCenterCode = (string)r["ExpCenterCode"];
                    productionInfo.ExpeditionLevel = (string)r["ExpeditionLevel"];
                    productionInfo.ExpeditionZone = (string)r["ExpZone"];
                    productionInfo.ExpeditionType = (string)r["ExpType"];

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

        public async Task<IEnumerable<ProdFileInfo>> GetProductionDetailPrinterReport(IPrintService print, int runID, int serviceCompanyID, int paperMediaID,
                    int stationMediaID, int expeditionType, string expCode, bool hasColorPages, int envMaterialID, int plexType)
        {
            string sql = @"RP_UX_PRODUCTION_REPORT";
            var parameters = new DynamicParameters();
            DataTable RunIDList = new DataTable();
            RunIDList.Columns.Add("ID", typeof(int));
            RunIDList.Rows.Add(runID);
            parameters.Add("RunIDList", RunIDList.AsTableValuedParameter("IDlist"));

            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);
            parameters.Add("PaperMediaID", paperMediaID, DbType.Int64);
            parameters.Add("StationMediaID", stationMediaID, DbType.Int64);
            parameters.Add("ExpeditionType", expeditionType, DbType.Int64);
            parameters.Add("ExpCode", expCode, DbType.String);
            parameters.Add("HasColorPages", hasColorPages, DbType.Boolean);
            parameters.Add("EnvMaterialID", envMaterialID, DbType.Int64);
            parameters.Add("PlexType", plexType, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                List<ProdFileInfo> FileList = new List<ProdFileInfo>();

                connection.Open();
                var obs = await connection.QueryAsync(sql, parameters,
                                    commandType: CommandType.StoredProcedure);
                var dt = _context.ToDataTable(obs);

                foreach (DataRow r in dt.Rows)
                {
                    ProdFileInfo ProdFile = new ProdFileInfo();
                    FileList.Add(ProdFile);
                    ProdFile.PlexCode = (string)r["PlexCode"];
                    ProdFile.RunID = (int)r["RunID"];
                    ProdFile.FileID = (int)r["FileID"];
                    ProdFile.FilePath = (string)r["FilePath"];
                    ProdFile.FileName = (string)r["FileName"];
                    ProdFile.ShortFileName = (string)r["ShortFileName"];
                    ProdFile.FilePrinterSpecs = (string)r["FilePrinterSpecs"];
                    ProdFile.FilePrintedFlag = (bool)r["FilePrintedFlag"];
                    int colorFeature = 0;
                    int plexFeature = 0;
                    print.GetPrinterFeatures(ProdFile.FilePrinterSpecs, ProdFile.PlexCode, ref colorFeature, ref plexFeature);
                    ProdFile.FileColor = colorFeature;
                    ProdFile.FilePlexType = plexFeature;

                    ProdFile.RegistDetailFileName = (string)r["RegistDetailFileName"];
                    ProdFile.RegistDetailShortFileName = (string)r["RegistDetailShortFileName"];
                    ProdFile.RegistDetailFilePrinterSpecs = (string)r["RegistDetailFilePrinterSpecs"];
                    ProdFile.RegistDetailFilePrintedFlag = (bool)r["RegistDetailFilePrintedFlag"];
                    colorFeature = 0;
                    plexFeature = 0;
                    print.GetPrinterFeatures(ProdFile.RegistDetailFilePrinterSpecs, ref colorFeature, ref plexFeature);
                    ProdFile.RegistDetailFileColor = colorFeature;
                    ProdFile.RegistDetailFilePlexType = plexFeature;

                    ProdFile.EnvMaterialID = envMaterialID;
                    ProdFile.EnvMaterialRef = (string)r["EnvMaterialRef"];

                    ProdFile.PrinterOperator = r["PrinterOperator"].ConvertFromDBVal<string>();
                    ProdFile.Printer = r["Printer"].ConvertFromDBVal<string>();
                    ProdFile.TotalPrint = (int)r["TotalPrint"];
                    ProdFile.StartSeqNum = (int)r["StartSeqNum"];
                    ProdFile.EndSeqNum = (int)r["EndSeqNum"];
                    ProdFile.TotalPostObjs = (int)r["TotalPostObjs"];
 
                    ProdFile.ExpLevel = (int)r["ExpLevel"];
                    ProdFile.ExpCenterCode = (string)r["ExpCenterCode"];
                    ProdFile.ExpeditionLevel = (string)r["ExpeditionLevel"];
                    ProdFile.ExpeditionZone = (string)r["ExpZone"];

                    for (int i = 21; i < dt.Columns.Count; i++)
                    {
                        string[] strings = dt.Columns[i].ColumnName.Split("|");
                        if (strings[0] == "Paper")
                        {
                            ProdFile.PaperTotals = ProdFile.PaperTotals ?? new Dictionary<string, int>();
                            ProdFile.PaperTotals.Add(strings[1], value: (int)r.ItemArray[i]);
                        }
                        else if (strings[0] == "Station")
                        {
                            ProdFile.StationTotals = ProdFile.StationTotals ?? new Dictionary<string, int>();
                            ProdFile.StationTotals.Add(strings[1], value: (int)r.ItemArray[i]);
                        }

                    }
                }
                return FileList;
            }
        }

        public async Task<IEnumerable<ProductionDetailInfo>> GetProductionReport(int runID, int serviceCompanyID)
        {
            string sql = @"RP_UX_PRODUCTION_SUBSET_REPORT";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);

            DataTable RunIDList = new DataTable();
            RunIDList.Columns.Add("ID", typeof(int));
            RunIDList.Rows.Add(runID);
            parameters.Add("RunIDList", RunIDList.AsTableValuedParameter("IDlist"));
            //parameters.Add("RunID", runID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                //pass all servicecompany runid

                IEnumerable<ProductionDetailInfo> productionSubsetReport = await connection.QueryAsync<ProductionDetailInfo>(sql, parameters,
                            commandType: CommandType.StoredProcedure);
                return productionSubsetReport;
            }
        }


        //FOR REFERENCE https://stackoverflow.com/questions/33087629/dapper-dynamic-parameters-with-table-valued-parameters
        public async Task<IEnumerable<ProductionRunInfo>> GetProductionRunReport(int ServiceCompanyID)
        {

            string sql = @"RP_UX_PRODUCTION_RUN_REPORT";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", ServiceCompanyID, DbType.Int64);

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

                IEnumerable<ProductionRunInfo> productionRunReport = await connection.QueryAsync<ProductionRunInfo>(sql, parameters, commandType: CommandType.StoredProcedure);
                return productionRunReport;
            }

        }

        public async Task<string> GetServiceCompanyCode(int serviceCompanyID)
        {
            string sql = @" SELECT CompanyCode
                            FROM RD_COMPANY
                            WHERE CompanyID = @ServiceCompanyID";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                //pass all servicecompany runid

                string serviceCompanyCode = await connection.QueryFirstOrDefaultAsync<string>(sql, parameters);
                return serviceCompanyCode;
            }
        }


    }
}
