using Dapper;
using Shared.Models.General;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Data.Repositories
{
    public class FullfilledFilesRepository : IFullfilledFilesRepository
    {
        private readonly DapperContext _context;
        public FullfilledFilesRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Result> RegistFullFill(string fileBarcode, string user, DataTable serviceCompanyList)
        {
            
            string sql = @"RT_UX_REGIST_FULLFILL";
            var parameters = new DynamicParameters();
            parameters.Add("FileBarcode", fileBarcode, DbType.String);
            parameters.Add("UserName", user, DbType.String);
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Result> results = await connection.QueryAsync<Result>(sql, parameters, 
                    commandType: CommandType.StoredProcedure);
                return results.First();
            }
        }
    }
}
