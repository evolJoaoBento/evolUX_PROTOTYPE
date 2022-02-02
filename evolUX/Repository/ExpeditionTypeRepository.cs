using Dapper;
using evolUX.Context;
using evolUX.Interfaces;
using evolUX.Models;

namespace evolUX.Repository
{
    public class ExpeditionTypeRepository : IExpeditionTypeRepository
    {
        private readonly DapperContext _context;
        public ExpeditionTypeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<ExpeditionType>> GetExpeditionTypes()
        {
            var expeditionTypeList = new List<ExpeditionType>();
            string sql = "SELECT * FROM RD_EXPEDITION_TYPE";
            
            using (var connection = _context.CreateConnection())
            {
                expeditionTypeList = (List<ExpeditionType>) await connection.QueryAsync<ExpeditionType>(sql);
                return expeditionTypeList;
            }
        }
    }
}
