using Dapper;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Data.Repositories
{
    public class ExpeditionZoneRepository : IExpeditionZoneRepository
    {
        private readonly DapperContext _context;
        public ExpeditionZoneRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<dynamic>> GetExpeditionZones()
        {
            var expeditionZoneList = new List<dynamic>();
            //TODO:
            string sql = $"SELECT ExpeditionZone as [id], " +
                            $"Description as [description] FROM RD_EXPEDITION_ZONE";
            
            using (var connection = _context.CreateConnectionEvolDP())
            {
                expeditionZoneList = (List<dynamic>) await connection.QueryAsync<dynamic>(sql);
                return expeditionZoneList;
            }
        }
    }
}
