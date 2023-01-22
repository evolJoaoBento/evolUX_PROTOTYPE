using Dapper;
using Shared.Models.Areas.Finishing;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using System.Data;
using System.Data.SqlClient;
using Shared.Models.General;
using Shared.Extensions;
using System.Transactions;

namespace evolUX.API.Data.Repositories
{
    public class PrintFilesRepository : IPrintFilesRepository
    {
        private readonly DapperContext _context;
        public PrintFilesRepository(DapperContext context)
        {
            _context = context;
        }


        //SUM TOTAL AND DYNAMICS
        //FOR REFERENCE https://www.faqcode4u.com/faq/530844/dapper-mapping-dynamic-pivot-columns-from-stored-procedure
        //              https://stackoverflow.com/questions/8229927/looping-through-each-element-in-a-datarow
        public async Task<IEnumerable<ResourceInfo>> GetPrinters(IEnumerable<int> profileList, string filesSpecs,
                    bool ignoreProfiles)
        {
            string sql = @"evolUX_RESOURCE_BY_FILTER";
            /*
             *                 -- 'RECOVER' - Recuperações
                               -- 'RDRECOVER' - RegistDetail recuperações
                               -- 'EXPEDITION' - Guias de Expedição
             */
            var parameters = new DynamicParameters();
            parameters.Add("ProfileList", profileList.toDataTable().AsTableValuedParameter("IDlist"));
            parameters.Add("ResName", "PRINTER", DbType.String);
            parameters.Add("ResValueFilter", filesSpecs, DbType.String);
            parameters.Add("IgnoreProfiles", ignoreProfiles, DbType.Boolean);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                //pass all servicecompany runid

                IEnumerable<ResourceInfo> printers = await connection.QueryAsync<ResourceInfo>(sql, parameters, commandType: CommandType.StoredProcedure);
                return printers.Where(printers => printers.MatchFilter == true);
            }
        }

        public async Task<FlowInfo> GetFlow(string serviceCompanyCode)
        {
            string sql = @" SELECT f.FlowID, f.DefaultPriority Priority, f.FlowName
                            FROM FLOWS f WITH(NOLOCK)
                            INNER JOIN
                                FLOWS_CRITERIA fType WITH(NOLOCK)
                            ON fType.FlowID = f.FlowID AND fType.CriteriaName = 'TYPE'
                            INNER JOIN
                                FLOWS_CRITERIA fServiceCompany WITH(NOLOCK)
                            ON fServiceCompany.FlowID = f.FlowID AND fServiceCompany.CriteriaName = 'SERVICECOMPANYCODE'
                            WHERE f.[Enable] = 1 -- Fluxo Ativo
                               AND fType.CriteriaValue = 'PRINT'
                            AND fServiceCompany.CriteriaValue = @ServiceCompanyCode
                            ORDER BY f.FlowID ASC";
            /*
             *                 -- 'RECOVER' - Recuperações
                               -- 'RDRECOVER' - RegistDetail recuperações
                               -- 'EXPEDITION' - Guias de Expedição
             */
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyCode", serviceCompanyCode, DbType.String);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                //pass all servicecompany runid

                FlowInfo flowInfo = await connection.QueryFirstOrDefaultAsync<FlowInfo>(sql, parameters);
                return flowInfo;
            }
        } 
        public async Task<IEnumerable<FlowParameter>> GetFlowParameters(int flowID)
        {
            string sql = @" SELECT  FlowID, ParameterName, ParameterValue
                            FROM    FLOWS_DATA
                            WHERE   FlowID = @FlowID
                            ";
            var parameters = new DynamicParameters();
            parameters.Add("FlowID", flowID, DbType.Int32);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                //pass all servicecompany runid

                IEnumerable<FlowParameter> flowParameters = await connection.QueryAsync<FlowParameter>(sql, parameters);
                return flowParameters;
            }
        }

        public async Task<Result> TryPrint(IEnumerable<FlowParameter> flowparameters, FlowInfo flowinfo, int userID)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                string sql = @"REGIST_NEW_JOB";
                var parameters = new DynamicParameters();
                parameters.Add("FlowID", flowinfo.FlowID, DbType.Int32);
                parameters.Add("Priority", flowinfo.Priority, DbType.Int32);
                parameters.Add("Description", flowinfo.FlowName, DbType.String);
                parameters.Add("UserID", userID, DbType.Int32);
                parameters.Add("@JobID", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                using (var connection = _context.CreateConnectionEvolFlow())
                {
                    //pass all servicecompany runid

                    await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                    int JobID = parameters.Get<int>("@JobID");

                    foreach (FlowParameter p in flowparameters)
                    {
                        sql = @"   INSERT INTO JOB_DATA(JobID, ParameterNr, ParameterName, ParameterValue)
                                SELECT @JobID, ISNULL(MAX(ParameterNr),0)+1, @ParameterName, (" + p.ParameterValue + @")
                                FROM JOB_DATA WHERE JobID = @JobID";
                        parameters = new DynamicParameters();
                        parameters.Add("JobID", JobID, DbType.Int32);
                        parameters.Add("ParameterName", p.ParameterName, DbType.String);
                        //sql = sql.Replace("@ParameterValue", "(" + p.ParameterValue + ")");
                        //parameters.Add("ParameterValue", p.ParameterValue, DbType.String);

                        int rowsAffected = await connection.ExecuteAsync(sql, parameters, commandType: CommandType.Text);
                        if (rowsAffected <= 0)
                        {
                            string s = "Parameter: " + p.ParameterName + " could not be registred!";
                            throw (new SystemException(s));
                        }
                    }

                    sql = "UPDATE JOBS SET StartTimestamp = CURRENT_TIMESTAMP WHERE JobID = @JobID";
                    parameters = new DynamicParameters();
                    parameters.Add("JobID", JobID, DbType.Int32);
                    int Affected = await connection.ExecuteAsync(sql, parameters, commandType: CommandType.Text);
                    if (Affected <= 0)
                    {
                        string s = "Job Timestamp could not be registred!";
                        throw (new SystemException(s));
                    }

                }
                transactionScope.Complete();
            }
            Result result = new Result();
            result.ErrorID = 0;
            result.Error = "Success";
            return result;
           
        }

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
    }
}
