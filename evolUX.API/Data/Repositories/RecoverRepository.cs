using Dapper;
using Shared.Models.General;
using Shared.Models.Areas.Finishing;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Data.Repositories
{
    public class RecoverRepository : IRecoverRepository
    {
        private readonly DapperContext _context;
        public RecoverRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Result>> RegistTotalRecover(string fileBarcode, string user, DataTable serviceCompanyList, bool permissionLevel)
        {
            
            string sql = @"EXEC RT_UX_REGIST_TOTAL_RECOVER  @FileBarcode
                                                            @Username
                                                            @ServiceCompanyList
                                                            @PermissionLevel";
            var parameters = new DynamicParameters();
            parameters.Add("FileBarcode", fileBarcode, DbType.String);
            parameters.Add("Username", user, DbType.String);
            parameters.Add("PermissionLevel", permissionLevel, DbType.Binary);
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Result> results = await connection.QueryAsync<Result>(sql, parameters, 
                    commandType: CommandType.StoredProcedure);
                return results;
            }
        }
        public async Task<IEnumerable<Result>> RegistPartialRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel)
        {
            
            string sql = @"EXEC RT_UX_REGIST_PARTIAL_RECOVER    @StartBarcode
                                                                @EndBarcode
                                                                @Username
                                                                @ServiceCompanyList
                                                                @PermissionLevel";
            var parameters = new DynamicParameters();
            parameters.Add("StartBarcode", startBarcode, DbType.String);
            parameters.Add("EndBarcode", endBarcode, DbType.String);
            parameters.Add("Username", user, DbType.String);
            parameters.Add("PermissionLevel", permissionLevel, DbType.Binary);
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Result> results = await connection.QueryAsync<Result>(sql, parameters, 
                    commandType: CommandType.StoredProcedure);
                return results;
            }
        }
        public async Task<IEnumerable<Result>> RegistDetailRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel)
        {

            string sql = @"EXEC RT_UX_REGIST_EXPCOMPANY_REGIST_DETAIL_RECOVER   @StartBarcode
                                                                                @EndBarcode
                                                                                @Username
                                                                                @ServiceCompanyList
                                                                                @PermissionLevel";
            var parameters = new DynamicParameters();
            parameters.Add("StartBarcode", startBarcode, DbType.String);
            parameters.Add("EndBarcode", endBarcode, DbType.String);
            parameters.Add("Username", user, DbType.String);
            parameters.Add("PermissionLevel", permissionLevel, DbType.Binary);
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Result> results = await connection.QueryAsync<Result>(sql, parameters,
                    commandType: CommandType.StoredProcedure);
                return results;
            }
        }

        public async Task<IEnumerable<PendingRecovery>> GetPendingRecoveries(int ServiceCompanyID)
        {
            string sql = @"EXEC RP_UX_SERVICECOMPANY_PENDING_RECOVER    @ServiceComapanyID";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceComapanyID", ServiceCompanyID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<PendingRecovery> pendingRecoveries = await connection.QueryAsync<PendingRecovery>(sql, parameters,
                    commandType: CommandType.StoredProcedure);
                return pendingRecoveries;
            }
        }
        public async Task<IEnumerable<PendingRecovery>> GetPendingRecoveriesRegistDetail(int ServiceCompanyID)
        {
            string sql = @"EXEC RP_UX_SERVICECOMPANY_REGIST_PENDING_RECOVER    @ServiceComapanyID";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceComapanyID", ServiceCompanyID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<PendingRecovery> pendingRecoveries = await connection.QueryAsync<PendingRecovery>(sql, parameters,
                    commandType: CommandType.StoredProcedure);
                return pendingRecoveries;
            }
        }
    }
}
