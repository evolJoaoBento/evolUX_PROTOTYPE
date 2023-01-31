using Dapper;
using Shared.Models.General;
using evolUX.API.Data.Context;
using System.Data;
using evolUX.API.Areas.Finishing.Repositories.Interfaces;

namespace evolUX.API.Areas.Finishing.Repositories
{
    public class PrintedFilesRepository : IPrintedFilesRepository
    {
        private readonly DapperContext _context;
        public PrintedFilesRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Result> RegistPrint(string fileBarcode, string user, DataTable serviceCompanyList)
        {
            string sql = @"RT_UX_REGIST_PRINT";
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
