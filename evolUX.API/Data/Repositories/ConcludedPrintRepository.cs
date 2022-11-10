using Dapper;
using SharedModels.Models.General;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Data.Repositories
{
    public class ConcludedPrintRepository : IConcludedPrintRepository
    {
        private readonly DapperContext _context;
        public ConcludedPrintRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Result>> RegistPrint(string fileBarcode, string user, DataTable serviceCompanyList)
        {
            
            string sql = @"EXEC RT_UX_REGIST_PRINT      @FileBarcode
                                                        @Username
                                                        @ServiceCompanyList";
            var parameters = new DynamicParameters();
            parameters.Add("FileBarcode", fileBarcode, DbType.String);
            parameters.Add("Username", user, DbType.String);
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Result> results = await connection.QueryAsync<Result>(sql, parameters, 
                    commandType: CommandType.StoredProcedure);
                return results;
            }
        }
    }
}
