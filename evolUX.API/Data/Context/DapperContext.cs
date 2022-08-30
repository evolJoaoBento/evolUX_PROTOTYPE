using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace evolUX.API.Data.Context
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

        public DataTable ToDataTable(IEnumerable<dynamic> items)
        {
            if (items == null) return null;
            var data = items.ToArray();
            if (data.Length == 0) return null;
            var dt = new DataTable();
            foreach (var pair in ((IDictionary<string, object>)data[0]))
            {
                dt.Columns.Add(pair.Key, (pair.Value ?? string.Empty).GetType());
            }
            foreach (var d in data)
            {
                dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            }
            return dt;
        }
    }
}
