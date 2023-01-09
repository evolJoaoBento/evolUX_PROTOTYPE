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


        public async Task<IEnumerable<int>> GetProfile(int user)
        {
            string sql = @" SELECT  ProfileID
                            FROM    [USER_PROFILES] up WITH(NOLOCK)
                            WHERE   up.UserID = @UserID";
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
            
            string sql = string.Format(@" SELECT  DISTINCT CompanyServer
                            FROM    [PROFILES]
                            WHERE   ProfileID in ({0})
                                        AND 
                                    CompanyServer is NOT NULL", profilesstr);
 
            using (var connection = _context.CreateConnectionEvolFlow())
            {
                IEnumerable<string> servers = await connection.QueryAsync<string>(sql);
                return servers;
            }
        }

        public async Task<DataTable> GetServiceCompanies(IEnumerable<string> servers)
        {
            string serversStr = servers.toCommaSeperatedFormatedString();

            string sql = string.Format(@"SELECT   DISTINCT CompanyID [ID]
                            FROM    RD_COMPANY c WITH(NOLOCK)
                            INNER JOIN
                                    RD_SERVICE_COMPANY_RESTRICTION scr WITH(NOLOCK)
                            ON      scr.ServiceCompanyID = c.CompanyID
                            WHERE   (c.CompanyServer in ({0})
                                OR EXISTS(  SELECT  *
                                            FROM    RD_COMPANY WITH(NOLOCK)
                                            WHERE   CompanyServer in ({0})
                                                AND CompanyID = 1))", serversStr);

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
                var result = await connection.QueryAsync<SideBarAction>(sql, parameters, commandType: CommandType.StoredProcedure);
                var obs = await connection.QueryAsync(sql);
                DataTable sideBardt = _context.ToDataTable(result);

                List<SideBarAction> sideBarActions = new List<SideBarAction>();
                if (sideBardt != null && sideBardt.Rows.Count > 0) 
                {
                    int LastActionIDLevel1 = -1;
                    int LastActionIDLevel2 = -1;
                    foreach (DataRow row in sideBardt.Rows) 
                    {

                    }
                }

                return sideBarActions;
            }

        }
    }
}
