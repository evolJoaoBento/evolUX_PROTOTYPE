using Dapper;
using evolUX.API.Areas.Finishing.Models;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
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


        public async Task<string> GetProfile(int user)
        {
            string sql = @" SELECT  ProfileID
                            FROM    [USER_PROFILES] up WITH(NOLOCK)
                            WHERE   up.UserID = @UserID";
            var parameters = new DynamicParameters();
            parameters.Add("UserID", user, DbType.Int64);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                string  profile = await connection.QuerySingleOrDefaultAsync<string>(sql, parameters);
                return profile;
            }
        }

        public async Task<IEnumerable<string>> GetServers(string profile)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            IEnumerable<int> list = new List<int>();//stick the thing here
            foreach (int item in list)
            {
                dt.Rows.Add("ID_" + item);
            }
            string sql = @" SELECT  DISTINCT CompanyServer
                            FROM    [PROFILES]
                            WHERE   ProfileID in @Profile
                                        AND 
                                    CompanyServer is NOT NULL";
            var parameters = new DynamicParameters();
            parameters.Add("Profile", profile, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<string> servers = await connection.QueryAsync<string>(sql, parameters);
                return servers;
            }
        }

        public async Task<DataTable> GetServiceCompanies(IEnumerable<string> servers)
        {
            string finalString = "";
            foreach (string server in servers)
            {
                finalString += "'"+server+"',";
            }
            finalString.Remove(finalString.Length - 1);

            string sql = @"SELECT   DISTINCT CompanyID [ID]
                            FROM    RD_COMPANY c WITH(NOLOCK)
                            INNER JOIN
                                    RD_SERVICE_COMPANY_RESTRICTION scr WITH(NOLOCK)
                            ON      scr.ServiceCompanyID = c.CompanyID
                            WHERE   (c.CompanyServer in (@l_COMPANYSERVERLIST)
                                OR EXISTS(  SELECT  *
                                            FROM    RD_COMPANY WITH(NOLOCK)
                                            WHERE   CompanyServer in (@l_COMPANYSERVERLIST)
                                                AND CompanyID = 1))";
            var parameters = new DynamicParameters();
            parameters.Add("l_COMPANYSERVERLIST", finalString, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                connection.Open();
                var obs = await connection.QueryAsync(sql, parameters);
                DataTable dt = _context.ToDataTable(obs);
                return dt;
            }
        }
    }
}
