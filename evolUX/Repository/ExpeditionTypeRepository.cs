using Dapper;
using evolUX.Context;
using evolUX.Interfaces;

namespace evolUX.Repository
{
    public class ExpeditionTypeRepository : IExpeditionTypeRepository
    {
        private readonly DapperContext _context;
        public ExpeditionTypeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<dynamic>> GetExpeditionTypes()
        {
            var expeditionTypeList = new List<dynamic>();
            string sql = $"SELECT ExpeditionType as [id], Priority as [priority], Description as [description] FROM RD_EXPEDITION_TYPE";
            
            using (var connection = _context.CreateConnection())
            {
                expeditionTypeList = (List<dynamic>) await connection.QueryAsync<dynamic>(sql);
                return expeditionTypeList;
            }
        }
    }
}
