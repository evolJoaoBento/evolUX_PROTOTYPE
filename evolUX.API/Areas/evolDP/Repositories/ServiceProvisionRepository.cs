using Dapper;
using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using evolUX.API.Data.Context;
using Shared.Models.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Repositories
{
    public class ServiceProvisionRepository : IServiceProvisionRepository
    {
        private readonly DapperContext _context;
        public ServiceProvisionRepository(DapperContext context)
        {
            _context = context;
        }

        //FOR REFERENCE https://stackoverflow.com/questions/33087629/dapper-dynamic-parameters-with-table-valued-parameters
        public async Task<IEnumerable<ServiceTask>> GetServiceTasks(int? serviceTaskID)
        {
            string sql = @"RD_UX_GET_SERVICE_TASK";
            var parameters = new DynamicParameters();
            if (serviceTaskID != null && serviceTaskID > 0)
                parameters.Add("ServiceTaskID", serviceTaskID, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ServiceTask> serviceTasks = await connection.QueryAsync<ServiceTask>(sql, parameters);
                return serviceTasks;
            }
        }
    }
}
