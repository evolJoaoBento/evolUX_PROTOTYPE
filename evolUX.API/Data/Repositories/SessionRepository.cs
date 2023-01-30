using Dapper;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using evolUX.API.Extensions;
using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Data.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DapperContext _context;
        public SessionRepository(DapperContext context)
        {
            _context = context;
        }

        public bool HasEvolDP ()
        { 
            return _context.HasEvolDP;
        }

        public async Task<IEnumerable<int>> GetProfile(int user)
        {
            string sql = @"SELECT ProfileID
                            FROM [USER_PROFILES] up WITH(NOLOCK)
                            WHERE up.UserID = @UserID";
            var parameters = new DynamicParameters();
            parameters.Add("UserID", user, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                IEnumerable<int> profiles = await connection.QueryAsync<int>(sql, parameters);
                return profiles;
            }
        }

        public async Task<IEnumerable<string>> GetServers(IEnumerable<int> profiles)
        {

            string profilesstr = profiles.toCommaSeperatedString();
            
            string sql = string.Format(@"SELECT DISTINCT CompanyServer
                            FROM [PROFILES]
                            WHERE ProfileID in ({0})
                                AND CompanyServer is NOT NULL", profilesstr);
 
            using (var connection = _context.CreateConnectionEvolFlow())
            {
                IEnumerable<string> servers = await connection.QueryAsync<string>(sql);
                return servers;
            }
        }

        public async Task<DataTable> GetCompanies(IEnumerable<string> servers, string CompanyType)
        {
            string serversStr = servers.toCommaSeperatedFormatedString();

            string sql = string.Format(@"SELECT DISTINCT c.CompanyID [ID]
                        FROM
                            RD_COMPANY c WITH(NOLOCK)
                        INNER JOIN
                            RD_COMPANY_CONFIG cc WITH(NOLOCK)
                        ON  cc.RelationCompanyID = c.CompanyID
                        WHERE cc.RelationType = '{1}'
                            AND (c.CompanyServer in ({0})
                                OR EXISTS(SELECT TOP 1 1
                                    FROM RD_COMPANY WITH(NOLOCK)
                                    WHERE CompanyServer in ({0})
                                        AND CompanyID = cc.CompanyID))", serversStr, CompanyType);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                connection.Open();
                var obs = await connection.QueryAsync(sql);
                DataTable dt = _context.ToDataTable(obs);
                return dt;
            }
        }

        public async Task<DataTable> GetCompanyBusinness(IEnumerable<string> servers, string CompanyType)
        {
            string serversStr = servers.toCommaSeperatedFormatedString();

            string sql = string.Format(@"SELECT DISTINCT b.BusinessID [ID]
                        FROM
                            RD_BUSINESS b WITH(NOLOCK)
                        INNER JOIN
	                        RD_COMPANY c WITH(NOLOCK)
                        ON b.CompanyID = c.CompanyID
                        LEFT OUTER JOIN
                            RD_COMPANY_CONFIG cc WITH(NOLOCK)
                        ON  cc.CompanyID = c.CompanyID
	                        AND cc.RelationType = '{1}'
                        WHERE c.CompanyServer in ({0})
                                OR EXISTS(SELECT TOP 1 1
                                    FROM RD_COMPANY WITH(NOLOCK)
                                    WHERE CompanyServer in ({0})
                                        AND CompanyID = cc.RelationCompanyID)", serversStr, CompanyType);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                connection.Open();
                var obs = await connection.QueryAsync(sql);
                DataTable dt = _context.ToDataTable(obs);
                return dt;
            }
        }
        public async Task<IEnumerable<SideBarAction>> GetSideBarActions(IEnumerable<int> profiles)
        {
            string sql = @"evolUX_GET_MENU";

            var parameters = new DynamicParameters();
            parameters.Add("ProfileList", profiles.toDataTable().AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                var result = await connection.QueryAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                DataTable sideBardt = _context.ToDataTable(result);

                List<SideBarAction> sideBarActions = new List<SideBarAction>();
                if (sideBardt != null && sideBardt.Rows.Count > 0) 
                {
                    int lastActionIDLevel1 = -1;
                    int lastActionIDLevel2 = -1;
                    int actionID;
                    string description;
                    string localizationKey;
                    string url;
                    foreach (DataRow row in sideBardt.Rows) 
                    {

                        if (int.TryParse(row["ActionIDLevel1"].ToString(),out actionID) && lastActionIDLevel1 != actionID)
                        {
                            description = row.IsNull("DescriptionLevel1") ? "" : row["DescriptionLevel1"].ToString();
                            localizationKey = row.IsNull("LocalizationKeyLevel1") ? "" : row["LocalizationKeyLevel1"].ToString();
                            sideBarActions.Add(new SideBarAction(actionID, description, localizationKey, ""));
                            lastActionIDLevel1 = actionID;
                        }
                        if (!row.IsNull("ActionIDLevel2"))
                        {
                            if (int.TryParse(row["ActionIDLevel2"].ToString(), out actionID) && lastActionIDLevel2 != actionID)
                            {
                                description = row.IsNull("DescriptionLevel2") ? "" : row["DescriptionLevel2"].ToString();
                                localizationKey = row.IsNull("LocalizationKeyLevel2") ? "" : row["LocalizationKeyLevel2"].ToString();
                                url = row.IsNull("URLLevel2") ? "" : row["URLLevel2"].ToString();
                                sideBarActions[sideBarActions.Count - 1].ChildActions.Add(new SideBarAction(actionID, description, localizationKey, url));
                                lastActionIDLevel2 = actionID;
                            }
                            if (!row.IsNull("ActionIDLevel3") && int.TryParse(row["ActionIDLevel3"].ToString(), out actionID))
                            {
                                description = row.IsNull("DescriptionLevel3") ? "" : row["DescriptionLevel3"].ToString();
                                localizationKey = row.IsNull("LocalizationKeyLevel3") ? "" : row["LocalizationKeyLevel3"].ToString();
                                url = row.IsNull("URLLevel3") ? "" : row["URLLevel3"].ToString();
                                sideBarActions[sideBarActions.Count - 1].ChildActions[sideBarActions[sideBarActions.Count - 1].ChildActions.Count - 1].ChildActions.Add(new SideBarAction(actionID, description, localizationKey, url));
                            }
                        }
                    }
                }

                return sideBarActions;
            }

        }
    }
}
