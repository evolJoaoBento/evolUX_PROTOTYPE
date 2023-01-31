using Dapper;
using Shared.Models.General;
using Shared.Models.Areas.Finishing;
using evolUX.API.Data.Context;
using evolUX.API.Areas.Finishing.Repositories.Interfaces;
using System.Data;

namespace evolUX.API.Areas.Finishing.Repositories
{
    public class PostalObjectRepository : IPostalObjectRepository
    {
        private readonly DapperContext _context;
        public PostalObjectRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<PostalObjectInfo> GetPostalObjectInfo(DataTable serviceCompanyList, string postObjBarcode)
        {

            string sql = @"RT_UX_GET_POSTAL_OBJECT_INFO";
            var parameters = new DynamicParameters();
            parameters.Add("PostObjBarcode", postObjBarcode, DbType.String);
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<PostalObjectInfo> results = await connection.QueryAsync<PostalObjectInfo>(sql, parameters,
                    commandType: CommandType.StoredProcedure);
                return results.First();
            }
        }
    }
}
