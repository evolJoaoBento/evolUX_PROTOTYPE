using Dapper;
using evolUX.API.Areas.Finishing.Repositories.Interfaces;
using evolUX.API.Data.Context;
using System.Data;

namespace evolUX.API.Areas.Finishing.Repositories
{
    public class FinishingRepository : IFinishingRepository
    {
        private readonly DapperContext _context;
        public FinishingRepository(DapperContext context)
        {
            _context = context;
        }
        public Task<dynamic> GetRunsOngoing()
        {
            dynamic obj = "";
            string sql = "";

            using (var connection = _context.CreateConnectionEvolDP())
            {

            }
            return obj;
        }

        public Task<dynamic> GetPendingRegist()
        {
            dynamic obj = "";
            string sql = "";

            using (var connection = _context.CreateConnectionEvolDP())
            {

            }
            return obj;
        }
    }
}
