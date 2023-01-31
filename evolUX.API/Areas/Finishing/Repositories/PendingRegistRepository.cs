using Dapper;
using Shared.Models.General;
using Shared.Models.Areas.Finishing;
using evolUX.API.Data.Context;
using evolUX.API.Areas.Finishing.Repositories.Interfaces;
using System.Data;

namespace evolUX.API.Areas.Finishing.Repositories
{
    public class PendingRegistRepository : IPendingRegistRepository
    {
        private readonly DapperContext _context;
        public PendingRegistRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PendingRegistInfo>> GetPendingRegist(DataTable serviceCompanyList)
        {

            string sql = @"RP_UX_SERVICECOMPANY_PENDING_REGIST";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<PendingRegistInfo> results = await connection.QueryAsync<PendingRegistInfo>(sql, parameters,
                    commandType: CommandType.StoredProcedure);
                return results;
            }
        }
        public async Task<PendingRegistDetailInfo> GetPendingRegistDetail(int RunID, DataTable serviceCompanyList)
        {
            PendingRegistDetailInfo result = new PendingRegistDetailInfo();

            var parameters = new DynamicParameters();
            parameters.Add("RunID", RunID, DbType.Int64);
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            string sql = @"RP_UX_SERVICECOMPANY_PENDING_REGIST_PRINT";
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<PendingRegistElement> results = await connection.QueryAsync<PendingRegistElement>(sql, parameters,
                    commandType: CommandType.StoredProcedure);
                if (results != null)
                    result.ToRegistPrintFiles = (List<PendingRegistElement>)results;
                else
                    result.ToRegistPrintFiles = new List<PendingRegistElement>();
            }

            sql = @"RP_UX_SERVICECOMPANY_PENDING_REGIST_FULLFILL";
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<PendingRegistElement> results = await connection.QueryAsync<PendingRegistElement>(sql, parameters,
                    commandType: CommandType.StoredProcedure);
                if (results != null)
                    result.ToRegistFullfillFiles = (List<PendingRegistElement>)results;
                else
                    result.ToRegistFullfillFiles = new List<PendingRegistElement>();
            }
            return result;
        }
    }
}
