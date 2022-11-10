using Dapper;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using evolUX.API.Extensions;
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
    }
}
