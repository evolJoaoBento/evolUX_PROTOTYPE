using Dapper;
using evolUX.API.Data.Context;
using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using System.Data;

namespace evolUX.API.Areas.evolDP.Repositories
{
    public class ExpeditionCompaniesRepository : IExpeditionCompaniesRepository
    {
        private readonly DapperContext _context;
        public ExpeditionCompaniesRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<dynamic>> GetExpeditionCompanies()//TODO: UNTESTED
        {
            var expeditionCompaniesList = new List<dynamic>();
            string sql = @"RD_UX_GET_EXPEDITION_COMPANIES";
            using (var connection = _context.CreateConnectionEvolDP())
            {
                expeditionCompaniesList = (List<dynamic>)await connection.QueryAsync(sql, commandType: CommandType.StoredProcedure);
                return expeditionCompaniesList;
            }
        }

        public async Task<List<dynamic>> GetExpeditionCompanyConfigs(dynamic data)//TODO: UNTESTED
        {
            var expeditionCompanyConfigsList = new List<dynamic>();
            string sql = @"SELECT 	EZ.Description as ZoneDescription,
	                                ET.Description as TypeDescription,
	                                EZ.ExpeditionZone as ExpeditionZone,
	                                ET.ExpeditionType as ExpeditionType
                           FROM	    (select Distinct ExpCompanyID , ExpeditionZone,ExpeditionType
	                       FROM 	RD_EXPCOMPANY_CONFIG) EC,
	                                RD_COMPANY C,
	                                RD_EXPEDITION_Zone EZ,
	                                RD_EXPEDITION_TYPE ET
                           WHERE 	EC.ExpCompanyID = C.CompanyID
	                                and
	                                EC.ExpeditionZone = EZ.ExpeditionZone
	                                and
	                                EC.ExpeditionType = ET.ExpeditionType
	                                and
                                    EC.ExpCompanyID = @ExpCompanyID";
            var parameters = new DynamicParameters();
            parameters.Add("ExpCompanyID", data.compId, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                expeditionCompanyConfigsList = (List<dynamic>)await connection.QueryAsync<dynamic>(sql, parameters);
                return expeditionCompanyConfigsList;
            }

        }

        public async Task<List<dynamic>> GetExpeditionCompanyConfigCharacteristics(dynamic data)//TODO: UNTESTED
        {
            var envelopeMediaGroupList = new List<dynamic>();
            string sql = @"SELECT	C.CompanyName as CompanyName,
	                                    EZ.Description ZoneDescription,
	                                    ET.Description TypeDescription,
	                                    CASE WHEN EC.MaxWeight is NULL THEN 'Sem limite' ELSE CAST(EC.MaxWeight as varchar) END as MaxWeight,
	                                    EC.StartDate as StartDate,
	                                    EC.UnitCost as UnitCost,
	                                    EC.ExpColumnB as Product,
	                                    EC.ExpColumnE as Velocity,
	                                    EC.ExpColumnI as SpecialServices 
                                FROM 	RD_EXPCOMPANY_CONFIG EC,
	                                    RD_COMPANY C,
	                                    RD_EXPEDITION_Zone EZ,
	                                    RD_EXPEDITION_TYPE ET
                                WHERE 	EC.ExpCompanyID=C.CompanyID
	                                    and
	                                    EC.ExpeditionZone=EZ.ExpeditionZone
	                                    and
	                                    EC.ExpeditionType=ET.ExpeditionType
	                                    and 
	                                    EC.ExpCompanyID = @COMPID
	                                    and
	                                    EC.ExpeditionZone = @EXPZONE
	                                    and
	                                    EC.ExpeditionType = @EXPTYPE
	                                    and
	                                    EC.StartDate = (SELECT MAX(StartDate) 
			                    FROM    [DMS_evolDP].[dbo].[RD_EXPCOMPANY_CONFIG] 
			                    WHERE   ExpCompanyID = EC.ExpCompanyID
			                            AND ExpeditionZone = EC.ExpeditionZone
			                            AND ExpeditionType = EC.ExpeditionType
			                            AND ExpCompanyLevel = EC.ExpCompanyLevel)";
            var parameters = new DynamicParameters();
            parameters.Add("COMPID", data.compId, DbType.String);
            parameters.Add("EXPZONE", data.expZone, DbType.String);
            parameters.Add("EXPTYPE", data.expType, DbType.String);
            using (var connection = _context.CreateConnectionEvolDP())
            {
                envelopeMediaGroupList = (List<dynamic>)await connection.QueryAsync<dynamic>(sql, parameters);
                return envelopeMediaGroupList;
            }

        }
    }
}
