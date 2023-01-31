using Dapper;
using Shared.Models.Areas.Finishing;
using Shared.Models.Areas.evolDP;
using evolUX.API.Data.Context;
using System.Data;
using System.Data.SqlClient;
using evolUX.API.Extensions;
using evolUX.API.Areas.Finishing.Repositories.Interfaces;

namespace evolUX.API.Areas.Finishing.Repositories
{
    public class PendingRecoverRepository : IPendingRecoverRepository
    {
        private readonly DapperContext _context;
        public PendingRecoverRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PendingRecoverElement>> GetPendingRecoverFiles(int serviceCompanyID)
        {
            string sql = @"RP_UX_SERVICECOMPANY_PENDING_RECOVER";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                //pass all servicecompany runid

                IEnumerable<PendingRecoverElement> pendingRecoverList = await connection.QueryAsync<PendingRecoverElement>(sql, parameters,
                            commandType: CommandType.StoredProcedure);
                return pendingRecoverList;
            }
        }

        public async Task<IEnumerable<PendingRecoverElement>> GetPendingRecoverRegistDetailFiles(int serviceCompanyID)
        {
            string sql = @"RP_UX_SERVICECOMPANY_PENDING_RECOVER_REGIST";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyID", serviceCompanyID, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                //pass all servicecompany runid

                IEnumerable<PendingRecoverElement> pendingRecoverList = await connection.QueryAsync<PendingRecoverElement>(sql, parameters,
                            commandType: CommandType.StoredProcedure);
                return pendingRecoverList;
            }
        }

        //FOR REFERENCE https://stackoverflow.com/questions/33087629/dapper-dynamic-parameters-with-table-valued-parameters
        public async Task<IEnumerable<Company>> GetServiceCompanies(DataTable ServiceCompanyList)
        {
            string sql = @"RD_UX_GET_COMPANIES_INFO";
            var parameters = new DynamicParameters();
            parameters.Add("CompanyList", ServiceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Company> serviceCompanies = await connection.QueryAsync<Company>(sql, parameters, commandType: CommandType.StoredProcedure);
                return serviceCompanies;
            }

        }

    }
}
