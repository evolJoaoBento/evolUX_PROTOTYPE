using Dapper;
using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using evolUX.API.Data.Context;
using Shared.Models.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Repositories
{
    public class ConsumablesRepository : IConsumablesRepository
    {
        private readonly DapperContext _context;
        public ConsumablesRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes(string fullFillMaterialCode)
        {
            string sql = @"RD_UX_GET_FULLFILL_MATERIALCODE";
            var parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(fullFillMaterialCode))
                parameters.Add("FullFillMaterialCode", fullFillMaterialCode, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<FulfillMaterialCode> result = await connection.QueryAsync<FulfillMaterialCode>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        
        public async Task<IEnumerable<EnvelopeMediaGroup>> GetEnvelopeMediaGroups(int? envMediaGroupID)
        {
            string sql = @"RD_UX_GET_ENVELOPE_MEDIA_GROUP";
            var parameters = new DynamicParameters();
            if (envMediaGroupID != null && envMediaGroupID > 0)
                parameters.Add("EnvMediaGroupID", envMediaGroupID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<EnvelopeMediaGroup> envMedia = await connection.QueryAsync<EnvelopeMediaGroup>(sql,
                    parameters);
                return envMedia;
            }
        }

        public async Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMedia(int? envMediaID)
        {
            string sql = @"RD_UX_GET_ENVELOPE_MEDIA";
            var parameters = new DynamicParameters();
            if (envMediaID != null && envMediaID > 0)
                parameters.Add("EnvMediaID", envMediaID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<EnvelopeMedia> envMedia = await connection.QueryAsync<EnvelopeMedia>(sql,
                    parameters);
                return envMedia;
            }
        }
    }
}
