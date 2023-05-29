using Dapper;
using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using evolUX.API.Data.Context;
using Shared.Models.Areas.evolDP;
using System.Data;

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
        
        public async Task<IEnumerable<MaterialType>> GetMaterialTypes()
        {
            string sql = @"RD_UX_GET_MATERIAL_TYPE";
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<MaterialType> result = await connection.QueryAsync<MaterialType>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<MaterialElement>> GetMaterialGroups(int groupID, string groupCode, int materialTypeID, string materialTypeCode, DataTable serviceCompanyList)
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
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));
            using (var connection = _context.CreateConnectionEvolDP())
            {
                List<MaterialElement> result = new List<MaterialElement>();

                connection.Open();
                var obs = await connection.QueryAsync(sql, parameters,
                                    commandType: CommandType.StoredProcedure);
                var dt = _context.ToDataTable(obs);
                if (dt != null)
                {
                    int lastgroupID = -1;
                    MaterialElement group = new MaterialElement();
                    List<MaterialCostElement> materialCost = new List<MaterialCostElement>();
                    foreach (DataRow r in dt.Rows)
                    {
                        groupID = (int)r["GroupID"];
                        if (lastgroupID != groupID)
                        {
                            group = new MaterialElement();
                            result.Add(group);
                            materialCost = new List<MaterialCostElement>();
                            group.CostList = materialCost;
                            group.GroupID = groupID;

                            group.MaterialTypeID = (int)r["MaterialTypeID"];
                            group.GroupCode = (string)r["GroupCode"];
                            if (!r.IsNull("GroupDescription"))
                                group.GroupDescription = (string)r["GroupDescription"];
                            if (!r.IsNull("MaterialWeight"))
                                group.MaterialWeight = double.Parse(r["MaterialWeight"].ToString());
                            if (!r.IsNull("FullFillSheets"))
                                group.FullFillSheets = (int)r["FullFillSheets"];
                            if (!r.IsNull("FullFillMaterialCode"))
                                group.FullFillMaterialCode = (string)r["FullFillMaterialCode"];
                            if (!r.IsNull("ExpeditionMinWeight"))
                                group.ExpeditionMinWeight = double.Parse(r["ExpeditionMinWeight"].ToString());

                            lastgroupID = groupID;
                        }
                        if (!r.IsNull("ServiceCompanyID"))
                        {
                            MaterialCostElement costElement = new MaterialCostElement();
                            ((List<MaterialCostElement>)group.CostList).Add(costElement);
                            costElement.GroupID = groupID;
                            costElement.ServiceCompanyID = (int)r["ServiceCompanyID"];
                            costElement.CostDate = (int)r["CostDate"];
                            costElement.MaterialBinPosition = int.Parse(r["MaterialBinPosition"].ToString());
                            costElement.MaterialCost = double.Parse(r["MaterialCost"].ToString());
                            if (!r.IsNull("ProviderCompanyID"))
                                costElement.ProviderCompanyID = (int)r["ProviderCompanyID"];
                            costElement.ServiceCompanyMaterialPosition = 0;
                            if (!r.IsNull("ServiceCompanyMaterialPosition"))
                                costElement.ServiceCompanyMaterialPosition = (int)r["ServiceCompanyMaterialPosition"];
                        }
                    }
                }
                //IEnumerable<MaterialElement> result = await connection.QueryAsync<MaterialElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        
        public async Task<int> SetMaterialGroup(MaterialElement group, DataTable serviceCompanyList)
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

            if (group.CostList != null && group.CostList.Count() > 0)
            {
                MaterialCostElement mCost = group.CostList.First();
                if (mCost.ServiceCompanyID > 0)
                    parameters.Add("ServiceCompanyID", mCost.ServiceCompanyID, DbType.Int64);
                if (mCost.ProviderCompanyID > 0)
                    parameters.Add("ProviderCompanyID", mCost.ProviderCompanyID, DbType.Int64);
                if (mCost.CostDate >= 0)
                    parameters.Add("CostDate", mCost.CostDate, DbType.Int64);
                if (mCost.MaterialCost >= 0)
                    parameters.Add("MaterialCost", mCost.MaterialCost, DbType.Double);
                if (mCost.MaterialBinPosition >= 0)
                    parameters.Add("MaterialBinPosition", mCost.MaterialBinPosition, DbType.Int32);
            }
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            parameters.Add("@ReturnID", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                int groupID = parameters.Get<int>("@ReturnID");
                return groupID;
            }
        }

        public async Task<IEnumerable<MaterialElement>> GetMaterials(int materialID, string materialRef, string materialCode, int groupID, int materialTypeID, string materialTypeCode, DataTable serviceCompanyList)
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
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));
            using (var connection = _context.CreateConnectionEvolDP())
            {
                List<MaterialElement> result = new List<MaterialElement>();

                connection.Open();
                var obs = await connection.QueryAsync(sql, parameters,
                                    commandType: CommandType.StoredProcedure);
                var dt = _context.ToDataTable(obs);
                if (dt != null)
                {
                    int lastMaterialID = - 1;
                    string value;
                    MaterialElement material = new MaterialElement();
                    List<MaterialCostElement> materialCost = new List<MaterialCostElement>();
                    foreach (DataRow r in dt.Rows)
                    {
                        materialID = (int)r["MaterialID"];
                        if (lastMaterialID != materialID)
                        {
                            material = new MaterialElement();
                            result.Add(material);
                            materialCost = new List<MaterialCostElement>();
                            material.CostList = materialCost;
                            material.MaterialID = materialID;

                            material.MaterialTypeID = (int)r["MaterialTypeID"];
                            material.MaterialCode = (string)r["MaterialCode"];
                            if (!r.IsNull("MaterialDescription"))
                                material.MaterialDescription = (string)r["MaterialDescription"];
                            if (!r.IsNull("MaterialWeight"))
                                material.MaterialWeight = double.Parse(r["MaterialWeight"].ToString());
                            material.MaterialRef = (string)r["MaterialRef"];
                            if (!r.IsNull("FullFillSheets"))
                                material.FullFillSheets = (int)r["FullFillSheets"]; 
                            if (!r.IsNull("FullFillMaterialCode"))
                                material.FullFillMaterialCode = (string)r["FullFillMaterialCode"];
                            if (!r.IsNull("ExpeditionMinWeight"))
                                material.ExpeditionMinWeight = double.Parse(r["ExpeditionMinWeight"].ToString());
                            if (!r.IsNull("GroupID"))
                                material.GroupID = (int)r["GroupID"];

                            lastMaterialID = materialID;
                        }
                        if (!r.IsNull("ServiceCompanyID"))
                        {
                            MaterialCostElement costElement = new MaterialCostElement();
                            ((List<MaterialCostElement>)material.CostList).Add(costElement);
                            costElement.MaterialID = materialID;
                            costElement.ServiceCompanyID = (int)r["ServiceCompanyID"];
                            costElement.CostDate = (int)r["CostDate"];
                            costElement.MaterialBinPosition = int.Parse(r["MaterialBinPosition"].ToString());
                            costElement.MaterialCost = double.Parse(r["MaterialCost"].ToString());
                            if (!r.IsNull("ProviderCompanyID"))
                                costElement.ProviderCompanyID = (int)r["ProviderCompanyID"];
                            costElement.ServiceCompanyMaterialPosition = 0;
                            if (!r.IsNull("ServiceCompanyMaterialPosition"))
                                costElement.ServiceCompanyMaterialPosition = (int)r["ServiceCompanyMaterialPosition"];
                        }
                    }
                }
                //IEnumerable<MaterialElement> result = await connection.QueryAsync<MaterialElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<int> SetMaterial(MaterialElement material, DataTable serviceCompanyList)
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

            if (material.CostList != null && material.CostList.Count() > 0)
            {
                MaterialCostElement mCost = material.CostList.First();
                if (mCost.ServiceCompanyID > 0)
                    parameters.Add("ServiceCompanyID", mCost.ServiceCompanyID, DbType.Int64);
                if (mCost.ProviderCompanyID > 0)
                    parameters.Add("ProviderCompanyID", mCost.ProviderCompanyID, DbType.Int64);
                if (mCost.CostDate >= 0)
                    parameters.Add("CostDate", mCost.CostDate, DbType.Int64);
                if (mCost.MaterialCost >= 0)
                    parameters.Add("MaterialCost", mCost.MaterialCost, DbType.Double);
                if (mCost.MaterialBinPosition >= 0)
                    parameters.Add("MaterialBinPosition", mCost.MaterialBinPosition, DbType.Int32);
            }
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));

            parameters.Add("@ReturnID", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                int materialID = parameters.Get<int>("@ReturnID");
                return materialID;
            }
        }
 
        public async Task<IEnumerable<MaterialCostElement>> GetMaterialCost(int materialID, DataTable serviceCompanyList)
        {
            string sql = @"RD_UX_GET_MATERIAL_COST";
            var parameters = new DynamicParameters();
            if (materialID > 0)
                parameters.Add("MaterialID", materialID, DbType.Int64);
            parameters.Add("ServiceCompanyList", serviceCompanyList.AsTableValuedParameter("IDlist"));
            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<MaterialCostElement> result = await connection.QueryAsync<MaterialCostElement>(sql, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<int> SetMaterialCost(MaterialCostElement material)
        {
            string sql = @"RD_UX_SET_MATERIAL";
            var parameters = new DynamicParameters();
            parameters.Add("MaterialID", material.MaterialID, DbType.Int64);
            parameters.Add("ServiceCompanyID", material.ServiceCompanyID, DbType.Int64);
            if (material.ProviderCompanyID > 0)
                parameters.Add("ProviderCompanyID", material.ProviderCompanyID, DbType.Int64);
            if (material.CostDate >= 0)
                parameters.Add("CostDate", material.CostDate, DbType.Int64);
            if (material.MaterialCost >= 0)
                parameters.Add("MaterialCost", material.MaterialCost, DbType.Double);
            if (material.MaterialBinPosition >= 0)
                parameters.Add("MaterialBinPosition", material.MaterialBinPosition, DbType.Int32);
            parameters.Add("@ReturnID", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                int materialID = parameters.Get<int>("@ReturnID");
                return materialID;
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
