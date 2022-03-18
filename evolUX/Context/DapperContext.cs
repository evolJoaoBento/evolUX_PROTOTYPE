using System.Data;
using System.Data.SqlClient;

namespace evolUX.Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionStringEvolDP;
        private readonly string _connectionStringEvolFlow;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionStringEvolDP = _configuration.GetConnectionString("EvolDPConnection");
            _connectionStringEvolFlow = _configuration.GetConnectionString("EvolFlowConnection");
        }
        public IDbConnection CreateConnectionEvolDP()
            => new SqlConnection(_connectionStringEvolDP);
        public IDbConnection CreateConnectionEvolFlow()
            => new SqlConnection(_connectionStringEvolFlow);
    }
}
