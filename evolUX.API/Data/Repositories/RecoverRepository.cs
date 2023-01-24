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

        public async Task<Result> RegistTotalRecover(string fileBarcode, string user, DataTable serviceCompanyList, bool permissionLevel)
        {
            string sql = @"RT_UX_REGIST_TOTAL_RECOVER";
            var parameters = new DynamicParameters();
            parameters.Add("FileBarcode", fileBarcode, DbType.String);
            parameters.Add("UserName", user, DbType.String);
            parameters.Add("PermissionLevel", permissionLevel, DbType.Boolean);
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Result> results = await connection.QueryAsync<Result>(sql, parameters, 
                    commandType: CommandType.StoredProcedure);
                return results.First();
            }
        }
        public async Task<Result> RegistPartialRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel)
        {
            string sql = @"RT_UX_REGIST_PARTIAL_RECOVER";
            var parameters = new DynamicParameters();
            parameters.Add("StartBarcode", startBarcode, DbType.String);
            parameters.Add("EndBarcode", endBarcode, DbType.String);
            parameters.Add("UserName", user, DbType.String);
            parameters.Add("PermissionLevel", permissionLevel, DbType.Boolean);
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Result> results = await connection.QueryAsync<Result>(sql, parameters, 
                    commandType: CommandType.StoredProcedure);
                return results.First();
            }
        }
        public async Task<Result> RegistDetailRecover(string startBarcode, string endBarcode, string user, DataTable serviceCompanyList, bool permissionLevel)
        {
            string sql = @"RT_UX_REGIST_EXPCOMPANY_REGIST_DETAIL_RECOVER";
            var parameters = new DynamicParameters();
            parameters.Add("StartBarcode", startBarcode, DbType.String);
            parameters.Add("EndBarcode", endBarcode, DbType.String);
            parameters.Add("UserName", user, DbType.String);
            parameters.Add("PermissionLevel", permissionLevel, DbType.Boolean);
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Result> results = await connection.QueryAsync<Result>(sql, parameters,
                    commandType: CommandType.StoredProcedure);
                return results.First();
            }
        }

        //public async Task<IEnumerable<PendingRecoverElement>> GetPendingRecoveries(int ServiceCompanyID)
        //{
        //    string sql = @"RP_UX_SERVICECOMPANY_PENDING_RECOVER";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("ServiceCompanyID", ServiceCompanyID, DbType.Int64);

        //    using (var connection = _context.CreateConnectionEvolDP())
        //    {
        //        IEnumerable<PendingRecoverElement> pendingRecoveries = await connection.QueryAsync<PendingRecoverElement>(sql, parameters,
        //            commandType: CommandType.StoredProcedure);
        //        return pendingRecoveries;
        //    }
        //}
        //public async Task<IEnumerable<PendingRecoverElement>> GetPendingRecoveriesRegistDetail(int ServiceCompanyID)
        //{
        //    string sql = @"RP_UX_SERVICECOMPANY_REGIST_PENDING_RECOVER";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("ServiceCompanyID", ServiceCompanyID, DbType.Int64);

        //    using (var connection = _context.CreateConnectionEvolDP())
        //    {
        //        IEnumerable<PendingRecoverElement> pendingRecoveries = await connection.QueryAsync<PendingRecoverElement>(sql, parameters,
        //            commandType: CommandType.StoredProcedure);
        //        return pendingRecoveries;
        //    }
        //}
    }
}
