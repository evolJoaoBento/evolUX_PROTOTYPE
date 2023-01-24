using Dapper;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using System.Data;
using System.Data.SqlClient;
using Shared.Models.General;
using Shared.Extensions;
using System.Transactions;
using Shared.Models.Areas.Core;

namespace evolUX.API.Data.Repositories
{
    public class RegistJobRepository : IRegistJobRepository
    {
        private readonly DapperContext _context;
        public RegistJobRepository(DapperContext context)
        {
            _context = context;
        }


        //SUM TOTAL AND DYNAMICS
        //FOR REFERENCE https://www.faqcode4u.com/faq/530844/dapper-mapping-dynamic-pivot-columns-from-stored-procedure
        //              https://stackoverflow.com/questions/8229927/looping-through-each-element-in-a-datarow
 
        public async Task<FlowInfo> GetFlowByCriteria(Dictionary<string, object> dictionary)
        {
            string sql = @" SELECT f.FlowID, f.DefaultPriority Priority, f.FlowName
                            FROM FLOWS f WITH(NOLOCK)";

            int i = 0;
            foreach(string Key in dictionary.Keys)
            {
                i++;
                sql += string.Format(@" INNER JOIN FLOWS_CRITERIA f{0} WITH(NOLOCK)
                            ON f{0}.FlowID = f.FlowID AND f{0}.CriteriaName = '{1}' AND f{0}.CriteriaValue = '{2}'", i, Key, dictionary[Key]);
            }
            sql += @" WHERE f.[Enable] = 1 -- Fluxo Ativo
                     ORDER BY f.FlowID ASC";
            /*
             *                 -- 'PRINT' - Impressão
             *                 -- 'RECOVER' - Recuperações
                               -- 'RDRECOVER' - RegistDetail recuperações
                               -- 'EXPEDITION' - Guias de Expedição
             */

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                FlowInfo flowInfo = await connection.QueryFirstOrDefaultAsync<FlowInfo>(sql);
                return flowInfo;
            }
        } 
        public async Task<IEnumerable<FlowParameter>> GetFlowData(int flowID)
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

        public async Task<Result> TryRegistJob(IEnumerable<FlowParameter> flowparameters, FlowInfo flowinfo, int userID)
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
        public async Task<IEnumerable<Job>> GetJobs(int flowID)
        {
            string sql = @"evolUX_GET_JOB_BY_FlowID";
            var parameters = new DynamicParameters();
            parameters.Add("FlowID", flowID, DbType.Int32);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                //pass all servicecompany runid

                IEnumerable<Job> jobs = await connection.QueryAsync<Job>(sql, parameters, commandType: CommandType.StoredProcedure);
                return jobs;
            }
        }
    }
}
