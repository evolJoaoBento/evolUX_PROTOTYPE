using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using Dapper;
using System.Data;
using evolUX.API.Areas.Core.ViewModels;
using Shared.Models.General;

namespace evolUX.API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;
        public UserRepository(DapperContext context)
        {
            _context = context;
        }


        public async Task<List<UserModel>> GetAllUsers()
        {
            List<UserModel> userList = new List<UserModel>();
            string sql = "evolUX_GET_USER";
            using (var connection = _context.CreateConnectionEvolFlow())
            {
                userList = (List<UserModel>)await connection.QueryAsync<UserModel>(sql,
                    commandType: CommandType.StoredProcedure);
                return userList;
            }
        }

        public async Task<UserModel> GetUserByUsername(string username)
        {
            UserModel user = new UserModel();
            string sql = "evolUX_GET_USER";
            var parameters = new DynamicParameters();
            parameters.Add("UserName", username ,DbType.String);
            using (var connection = _context.CreateConnectionEvolFlow())
            {
                user = await connection.QuerySingleOrDefaultAsync<UserModel>(sql, parameters,
                    commandType: CommandType.StoredProcedure);
                return user;
            }
        }

        public async Task<List<RolesModel>> GetRolesByUsername(string username)
        {
            var roleList = new List<RolesModel>();
            string sql = @" SELECT p.* 
                            FROM
                                USERS u WITH(NOLOCK)
                            INNER JOIN
                                USER_PROFILES up WITH(NOLOCK)
                            ON u.UserID = up.UserID
                            INNER JOIN
                                PROFILES p WITH(NOLOCK)
                            ON up.ProfileID = p.ProfileID
                            WHERE u.UserName = @UserName AND u.Active = 1";

            var parameters = new DynamicParameters();
            parameters.Add("UserName", username, DbType.String);
            using (var connection = _context.CreateConnectionEvolFlow())
            {
                roleList = (List<RolesModel>)await connection.QueryAsync<RolesModel>(sql, parameters);
                return roleList;
            }
        }

        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserRefreshToken(string username, string refreshToken)
        {
            var query = "UPDATE USERS SET RefreshToken = @RefreshToken WHERE UserName = @UserName";
            var parameters = new DynamicParameters();
            parameters.Add("RefreshToken", refreshToken, DbType.String);
            parameters.Add("UserName", username, DbType.String);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task UpdateUserRefreshTokenAndTime(UserModel user)
        {
            var query = "UPDATE USERS SET RefreshToken = @RefreshToken, RefreshTokenExpiryTime = @RefreshTokenExpiryTime WHERE UserName = @UserName";
            var parameters = new DynamicParameters();
            parameters.Add("RefreshToken", user.RefreshToken, DbType.String);
            parameters.Add("RefreshTokenExpiryTime", user.RefreshTokenExpiryTime, DbType.DateTime2);
            parameters.Add("UserName", user.UserName, DbType.String);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteRefreshToken(string username)
        {
            var query = "UPDATE Users SET RefreshToken = NULL WHERE UserName = @UserName";
            var parameters = new DynamicParameters();
            parameters.Add("UserName", username, DbType.String);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task ChangeCulture(string culture)
        {
            var query = "evolUX_UPDATE_USER_INFO";
            var parameters = new DynamicParameters();
            parameters.Add("Language", culture, DbType.String);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
