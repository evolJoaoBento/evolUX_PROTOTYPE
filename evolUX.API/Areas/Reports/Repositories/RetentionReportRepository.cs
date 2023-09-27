using Dapper;
using Shared.Models.Areas.Reports;
using evolUX.API.Data.Context;
using System.Data;
using System.Data.SqlClient;
using evolUX.API.Extensions;
using evolUX.API.Areas.Reports.Repositories.Interfaces;
using evolUX.API.Models;
using System.Drawing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using NLog.Targets;

namespace evolUX.API.Areas.Reports.Repositories
{
    public class RetentionReportRepository : IRetentionReportRepository
    {
        private readonly DapperContext _context;
        public RetentionReportRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RetentionInfo>> GetRetentionReport(DataTable runIDList, int BusinessAreaID)
        {
            string sql = @"RPC_UX_RETENTION_LIST";
            var parameters = new DynamicParameters();
            parameters.Add("RunID", runIDList.Rows[0][0], DbType.Int64);
            parameters.Add("BusinessID", BusinessAreaID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {

                IEnumerable<RetentionInfo> retentionReport = await connection.QueryAsync<RetentionInfo>(sql, parameters,
                            commandType: CommandType.StoredProcedure);
                return retentionReport;
            }
        }


        //FOR REFERENCE https://stackoverflow.com/questions/33087629/dapper-dynamic-parameters-with-table-valued-parameters
        public async Task<IEnumerable<RetentionRunInfo>> GetRetentionRunReport(int BusinessAreaID, int RefDate)
        {

            string sql = @"RPC_UX_RETENTION_RESUME";
            var parameters = new DynamicParameters();

            using (var connection = _context.CreateConnectionEvolDP())
            {

                IEnumerable<RetentionRunInfo> retentionRunReport = await connection.QueryAsync<RetentionRunInfo>(sql, parameters, 
                    commandType: CommandType.StoredProcedure);
                return retentionRunReport;
            }

        }

        public async Task<RetentionInfoInfo> GetRetentionInfoReport(int RunID, int FileID, int SetID, int DocID)
        {
            string sql = @"RPC_UX_DOCUMENT_STATUS";
            var parameters = new DynamicParameters();
            parameters.Add("RunID", RunID, DbType.Int64);
            parameters.Add("FileID", FileID, DbType.Int64);
            parameters.Add("SetID", SetID, DbType.Int64);
            parameters.Add("DocID", DocID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                RetentionInfoInfo retentionInfoReport = await connection.QueryFirstOrDefaultAsync<RetentionInfoInfo>(sql, parameters,
                            commandType: CommandType.StoredProcedure);
                return retentionInfoReport;
            }
        }
    }
    public class DependentProductionRepository : IDependentProductionRepository
    {
        private readonly DapperContext _context;
        public DependentProductionRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DependentPrintsInfo>> GetDependentPrintsProduction(DataTable ServiceCompanyList)
        {
            string sql = @"RPC_UX_SERVICECOMPANY_PENDING_WORK_PRINT";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyList", ServiceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {

                IEnumerable<DependentPrintsInfo> dependentPrints = await connection.QueryAsync<DependentPrintsInfo>(sql, parameters,
                            commandType: CommandType.StoredProcedure);
                return dependentPrints;
            }
        }
        public async Task<IEnumerable<DependentFullfillInfo>> GetDependentFullfillProduction(DataTable ServiceCompanyList)
        {
            string sql = @"RPC_UX_SERVICECOMPANY_PENDING_WORK_FULLFILL";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyList", ServiceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {

                IEnumerable<DependentFullfillInfo> dependentFullFill = await connection.QueryAsync<DependentFullfillInfo>(sql, parameters,
                            commandType: CommandType.StoredProcedure);
                return dependentFullFill;
            }
        }
    }
}
