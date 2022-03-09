using evolUX.Context;
using evolUX.Interfaces;

namespace evolUX.Repository
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
            dynamic obj="";
            string sql = "";

            using (var connection = _context.CreateConnection())
            {
                
            }
            return obj;
        }
    }
}
