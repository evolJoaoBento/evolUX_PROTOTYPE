using Dapper;
using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using evolUX.API.Data.Context;
using Shared.Models.Areas.evolDP;
using System.Data;
using System.Text.RegularExpressions;

namespace evolUX.API.Areas.evolDP.Repositories
{
    public class MaterialsRepository : IMaterialsRepository
    {
        private readonly DapperContext _context;
        public MaterialsRepository(DapperContext context)
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
        
        public async Task<IEnumerable<MaterialType>> GetMaterialTypes(bool groupCodes, string materialTypeCode)
        {
            string sql = @"RD_UX_GET_MATERIAL_TYPE";
            var parameters = new DynamicParameters();
            parameters.Add("GroupCodes", groupCodes, DbType.Boolean);
            parameters.Add("MaterialTypeCode", materialTypeCode, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<MaterialType> result = await connection.QueryAsync<MaterialType>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<IEnumerable<MaterialElement>> GetMaterialGroups(int groupID, string groupCode, int materialTypeID, string materialTypeCode)
        {
            string sql = @"RD_UX_GET_MATERIAL_GROUP";
            var parameters = new DynamicParameters();
            if (groupID > 0)
                parameters.Add("GroupID", groupID, DbType.Int64);
            if (!string.IsNullOrEmpty(groupCode))
                parameters.Add("GroupCode", groupCode, DbType.String);
            if (materialTypeID > 0)
                parameters.Add("MaterialTypeID", materialTypeID, DbType.Int64);
            if (!string.IsNullOrEmpty(materialTypeCode))
                parameters.Add("MaterialTypeCode", materialTypeCode, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<MaterialElement> result = await connection.QueryAsync<MaterialElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        
        public async Task SetMaterialGroup(MaterialElement group)
        {
            string sql = @"RD_UX_SET_MATERIAL_GROUP";
            var parameters = new DynamicParameters();
            if (group.GroupID > 0)
                parameters.Add("GroupID", group.GroupID, DbType.Int64);
            parameters.Add("GroupCode", group.GroupCode, DbType.String);
            parameters.Add("GroupDescription", group.GroupDescription, DbType.String);
            parameters.Add("MaterialTypeID", group.MaterialTypeID, DbType.Int64);
            if (group.MaterialWeight >= 0)
                parameters.Add("MaterialWeight", group.MaterialWeight, DbType.Double);
            if (group.FullFillSheets >= 0)
                parameters.Add("FullFillSheets", group.FullFillSheets, DbType.Int64);
            if (!string.IsNullOrEmpty(group.FullFillMaterialCode))
                parameters.Add("FullFillMaterialCode", group.FullFillMaterialCode, DbType.String);
            if (group.ExpeditionMinWeight >= 0)
                parameters.Add("ExpeditionMinWeight", group.ExpeditionMinWeight, DbType.Double);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.QueryAsync<MaterialElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return;
            }
        }

        public async Task<IEnumerable<MaterialElement>> GetMaterials(int materialID, string materialRef, string materialCode, int groupID, int materialTypeID, string materialTypeCode)
        {
            string sql = @"RD_UX_GET_MATERIAL";
            var parameters = new DynamicParameters();
            if (materialID > 0)
                parameters.Add("MaterialID", materialID, DbType.Int64);
            if (!string.IsNullOrEmpty(materialRef))
                parameters.Add("MaterialRef", materialRef, DbType.String);
            if (!string.IsNullOrEmpty(materialCode))
                parameters.Add("MaterialCode", materialCode, DbType.String);
            if (groupID > 0)
                parameters.Add("GroupID", groupID, DbType.Int64);
            if (materialTypeID > 0)
                parameters.Add("MaterialTypeID", materialTypeID, DbType.Int64);
            if (!string.IsNullOrEmpty(materialTypeCode))
                parameters.Add("MaterialTypeCode", materialTypeCode, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<MaterialElement> result = await connection.QueryAsync<MaterialElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task SetMaterial(MaterialElement material)
        {
            string sql = @"RD_UX_SET_MATERIAL";
            var parameters = new DynamicParameters();
            if (material.MaterialID > 0)
                parameters.Add("MaterialID", material.MaterialID, DbType.Int64);
            parameters.Add("MaterialRef", material.MaterialRef, DbType.String);
            parameters.Add("MaterialCode", material.MaterialCode, DbType.String);
            parameters.Add("MaterialDescription", material.MaterialDescription, DbType.String);
            parameters.Add("MaterialTypeID", material.MaterialTypeID, DbType.Int64);
            if (material.MaterialWeight >= 0)
                parameters.Add("MaterialWeight", material.MaterialWeight, DbType.Double);
            if (material.FullFillSheets >= 0)
                parameters.Add("FullFillSheets", material.FullFillSheets, DbType.Int64);
            if (!string.IsNullOrEmpty(material.FullFillMaterialCode))
                parameters.Add("FullFillMaterialCode", material.FullFillMaterialCode, DbType.String);
            if (material.ExpeditionMinWeight >= 0)
                parameters.Add("ExpeditionMinWeight", material.ExpeditionMinWeight, DbType.Double);
            if (material.GroupID > 0)
                parameters.Add("GroupID", material.GroupID, DbType.Int64);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.QueryAsync<MaterialElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return;
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
