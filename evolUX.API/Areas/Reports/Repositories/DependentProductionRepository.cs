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
