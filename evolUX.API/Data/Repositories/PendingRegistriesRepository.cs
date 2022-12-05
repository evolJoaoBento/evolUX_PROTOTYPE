using Dapper;
using Shared.Models.General;
using Shared.Models.Areas.Finishing;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Data.Repositories
{
    public class PendingRegistriesRepository : IPendingRegistriesRepository
    {
        private readonly DapperContext _context;
        public PendingRegistriesRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PendingRegistry>> GetPendingRegistries(DataTable serviceCompanyList)
        {
            
            string sql = @"RT_UX_SERVICECOMPANY_PENDING_REGIST";
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<PendingRegistry> results = await connection.QueryAsync<PendingRegistry>(sql, parameters, 
                    commandType: CommandType.StoredProcedure);
                return results;
            }
        }
        
    }
}
