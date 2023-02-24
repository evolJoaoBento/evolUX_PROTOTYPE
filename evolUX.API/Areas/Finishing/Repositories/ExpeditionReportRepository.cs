using Dapper;
using Shared.Models.Areas.Finishing;
using Shared.Models.Areas.evolDP;
using evolUX.API.Data.Context;
using System.Data;
using System.Data.SqlClient;
using evolUX.API.Extensions;
using evolUX.API.Areas.Finishing.Repositories.Interfaces;
using evolUX.API.Models;

namespace evolUX.API.Areas.Finishing.Repositories
{
    public class ExpeditionReportRepository : IExpeditionReportRepository
    {
        private readonly DapperContext _context;
        public ExpeditionReportRepository(DapperContext context)
        {
            _context = context;
        }

        //FOR REFERENCE https://stackoverflow.com/questions/33087629/dapper-dynamic-parameters-with-table-valued-parameters
        public async Task<IEnumerable<Business>> GetCompanyBusiness(DataTable CompanyBusinessList)
        {
            string sql = @"RD_UX_GET_BUSINESS_INFO";
            var parameters = new DynamicParameters();
            parameters.Add("CompanyBusinessList", CompanyBusinessList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Business> companyBusiness = await connection.QueryAsync<Business>(sql, parameters, commandType: CommandType.StoredProcedure);
                return companyBusiness;
            }

        }

        public async Task<IEnumerable<ExpServiceCompanyFileElement>> GetPendingExpeditionFiles(int BusinessID, DataTable ServiceCompanyList)
        {
            string sql = @"RP_UX_GET_PENDING_EXPEDITION_FILES";
            var parameters = new DynamicParameters();
            parameters.Add("BusinessID", BusinessID, DbType.Int64);
            parameters.Add("ServiceCompanyList", ServiceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                List<ExpServiceCompanyFileElement> expeditionList = new List<ExpServiceCompanyFileElement>();

                connection.Open();
                var obs = await connection.QueryAsync(sql, parameters,
                                    commandType: CommandType.StoredProcedure);
                var dt = _context.ToDataTable(obs);
                if (dt != null)
                {
                    int lastServiceCompanyID = -1;
                    int lastExpCompanyID = -1;
                    int lastBusinessID = -1;
                    int lastContractID = -1;
                    int lastRunID = -1;
                    ExpServiceCompanyFileElement expServiceCompanyFile = new ExpServiceCompanyFileElement();
                    ExpCompanyFileElement expCompanyFile = new ExpCompanyFileElement();
                    ExpBusinessFileElement expBusinessFile = new ExpBusinessFileElement();
                    ExpContractFileElement expContractFile = new ExpContractFileElement();
                    ExpRunFileElement expRunFile = new ExpRunFileElement();

                    foreach (DataRow r in dt.Rows)
                    {
                        int expServiceCompanyID = (int)r["ServiceCompanyID"];
                        if (lastServiceCompanyID != expServiceCompanyID)
                        {
                            expServiceCompanyFile = new ExpServiceCompanyFileElement();
                            expeditionList.Add(expServiceCompanyFile);

                            expServiceCompanyFile.ServiceCompanyID = expServiceCompanyID;
                            expServiceCompanyFile.ServiceCompanyCode = (string)r["ServiceCompanyCode"];
                            expServiceCompanyFile.ServiceCompanyName = (string)r["ServiceCompanyName"];
                            lastServiceCompanyID = expServiceCompanyID;
                        }
                        int expCompanyID = (int)r["ExpCompanyID"];
                        if (lastExpCompanyID != expCompanyID)
                        {
                            expCompanyFile = new ExpCompanyFileElement();
                            expServiceCompanyFile.ExpCompanyList.Add(expCompanyFile);

                            expCompanyFile.ExpCompanyID = expCompanyID;
                            expCompanyFile.ExpCompanyCode = (string)r["ExpCompanyCode"];
                            lastExpCompanyID = expCompanyID;
                        }
                        int businessID = (int)r["BusinessID"];
                        if (lastBusinessID != businessID)
                        {
                            expBusinessFile = new ExpBusinessFileElement();
                            expCompanyFile.BusinessList.Add(expBusinessFile);

                            expBusinessFile.BusinessID = businessID;
                            expBusinessFile.BusinessCode = (string)r["BusinessCode"];
                            expBusinessFile.BusinessDescription = (string)r["BusinessDescription"];
                            expBusinessFile.CompanyID = (int)r["CompanyID"];
                            lastBusinessID = businessID;
                        }
                        int runID = (int)r["RunID"];
                        if (lastRunID != runID)
                        {
                            expRunFile = new ExpRunFileElement();
                            expBusinessFile.RunList.Add(expRunFile);

                            expRunFile.RunID = runID;
                            expRunFile.RunDate = (int)r["RunDate"];
                            expRunFile.RunSequence = Int32.Parse(r["RunSequence"].ToString());
                            lastRunID = runID;
                        }
                        int contractID = (int)r["ContractID"];
                        if (lastContractID != contractID)
                        {
                            expContractFile = new ExpContractFileElement();
                            expRunFile.ContractList.Add(expContractFile);

                            expContractFile.ContractID = contractID;
                            expContractFile.ContractNr = (int)r["ContractNr"];
                            expContractFile.ClientNr = (int)r["ClientNr"];
                            expContractFile.ClientName = (string)r["ClientName"];
                            lastContractID = contractID;
                        }
                        FileBase expFile = new FileBase();
                        expContractFile.FileList.Add(expFile);
                        expFile.RunID = runID;
                        expFile.FileID = (int)r["FileID"];
                        expFile.FileName = (string)r["FileName"];
                    }
                }
                return expeditionList;
            }
        }

        public async Task<int> RegistFileList(DataTable fileList, string userName)
        {
            string sql = @"RT_REGIST_FILE_EXPEDITION_REPORT_LIST";
            var parameters = new DynamicParameters();
            parameters.Add("FileList", fileList.AsTableValuedParameter("IDPairlist"));
            parameters.Add("@RequestID", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                int RequestID = parameters.Get<int>("@RequestID");
                return RequestID;
            }

        }

        public async Task<IEnumerable<ExpServiceCompanyFileElement>> GetExpeditionReportList(int BusinessID, DataTable ServiceCompanyList)
        {
            string sql = @"RP_UX_GET_EXPEDITION_REPORTS";
            var parameters = new DynamicParameters();
            parameters.Add("BusinessID", BusinessID, DbType.Int64);
            parameters.Add("ServiceCompanyList", ServiceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                List<ExpServiceCompanyFileElement> expeditionList = new List<ExpServiceCompanyFileElement>();

                connection.Open();
                var obs = await connection.QueryAsync(sql, parameters,
                                    commandType: CommandType.StoredProcedure);
                var dt = _context.ToDataTable(obs);
                if (dt != null)
                {
                    int lastServiceCompanyID = -1;
                    int lastExpCompanyID = -1;
                    int lastBusinessID = -1;
                    ExpServiceCompanyFileElement expServiceCompanyFile = new ExpServiceCompanyFileElement();
                    ExpCompanyFileElement expCompanyFile = new ExpCompanyFileElement();
                    ExpBusinessFileElement expBusinessFile = new ExpBusinessFileElement();

                    foreach (DataRow r in dt.Rows)
                    {
                        int expServiceCompanyID = (int)r["ServiceCompanyID"];
                        if (lastServiceCompanyID != expServiceCompanyID)
                        {
                            expServiceCompanyFile = new ExpServiceCompanyFileElement();
                            expeditionList.Add(expServiceCompanyFile);

                            expServiceCompanyFile.ServiceCompanyID = expServiceCompanyID;
                            expServiceCompanyFile.ServiceCompanyCode = (string)r["ServiceCompanyCode"];
                            expServiceCompanyFile.ServiceCompanyName = (string)r["ServiceCompanyName"];
                            lastServiceCompanyID = expServiceCompanyID;
                        }
                        int expCompanyID = (int)r["ExpCompanyID"];
                        if (lastExpCompanyID != expCompanyID)
                        {
                            expCompanyFile = new ExpCompanyFileElement();
                            expServiceCompanyFile.ExpCompanyList.Add(expCompanyFile);

                            expCompanyFile.ExpCompanyID = expCompanyID;
                            expCompanyFile.ExpCompanyCode = (string)r["ExpCompanyCode"];
                            lastExpCompanyID = expCompanyID;
                        }
                        int businessID = (int)r["BusinessID"];
                        if (lastBusinessID != businessID)
                        {
                            expBusinessFile = new ExpBusinessFileElement();
                            expCompanyFile.BusinessList.Add(expBusinessFile);

                            expBusinessFile.BusinessID = businessID;
                            expBusinessFile.BusinessCode = (string)r["BusinessCode"];
                            expBusinessFile.BusinessDescription = (string)r["BusinessDescription"];
                            expBusinessFile.CompanyID = (int)r["CompanyID"];
                            lastBusinessID = businessID;
                        }

                        ExpReportElement expReport = new ExpReportElement();
                        expBusinessFile.ReportList.Add(expReport);
                        expReport.ExpReportID = (int)r["ExpReportID"];
                        expReport.ExpRegistReportID = !r.IsNull("ExpRegistReportID") ? Convert.ToString(r["ExpRegistReportID"]) : string.Empty;
                        expReport.ExpReportNr = Int32.Parse(r["ExpReportNr"].ToString());
                        expReport.ExpTimeDate = (string)r["ExpTimeDate"];
                        expReport.ExpTime = (string)r["ExpTime"];
                        expReport.ExpTimeStamp = (DateTime)r["ExpTimeStamp"];

                        expReport.ExpContract.ContractID = (int)r["ContractID"];
                        expReport.ExpContract.ContractNr = (int)r["ContractNr"];
                        expReport.ExpContract.ClientNr = (int)r["ClientNr"];
                        expReport.ExpContract.ClientName = (string)r["ClientName"];

                        sql = @"RP_UX_GET_EXPEDITION_REPORT_FILES";
                        parameters = new DynamicParameters();
                        parameters.Add("ExpReportID", expReport.ExpReportID, DbType.Int64);

                        var fResult = await connection.QueryAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                        var fileDT = _context.ToDataTable(fResult);
                        if (fileDT != null)
                        {
                            int lastRunID = 0;
                            int lastFileID = 0;
                            ExpFileInfo expFile = new ExpFileInfo();
                            foreach (DataRow f in fileDT.Rows)
                            {
                                int RunID = (int)f["RunID"];
                                int FileID = (int)f["FileID"];
                                if (lastRunID != RunID || lastFileID != FileID)
                                {
                                    expFile = new ExpFileInfo();
                                    expReport.ExpFileList.Add(expFile);

                                    expFile.RunID = RunID;
                                    expFile.FileID = FileID;
                                    expFile.FileName = (string)f["FileName"];
                                    expFile.TotalPostObjs = (int)f["TotalPostObjs"];
                                    lastRunID = RunID;
                                    lastFileID = FileID;
                                }
                                expFile.ExpCompanyLevels.Add(new ExpLevelInfo((int)f["ExpCompanyLevel"], f.IsNull("MaxWeight") ? -1 : (int)f["MaxWeight"], (string)f["ExpeditionLevelDesc"], (int)f["PostalObjects"]));
                            }
                        }
                    }
                }
                return expeditionList;
            }
        }
        
    }
}
