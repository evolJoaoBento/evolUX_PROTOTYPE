using Dapper;
using evolUX.API.Areas.EvolDP.Repositories.Interfaces;
using evolUX.API.Data.Context;
using System.Data;

namespace evolUX.API.Areas.EvolDP.Repositories
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
            string sql = $"SELECT ExpeditionType as [id], " +
                            $"Priority as [priority], " +
                            $"Description as [description] FROM RD_EXPEDITION_TYPE";

            using (var connection = _context.CreateConnectionEvolDP())
            {
                expeditionTypeList = (List<dynamic>)await connection.QueryAsync<dynamic>(sql);
                return expeditionTypeList;
            }
        }
    }
}
