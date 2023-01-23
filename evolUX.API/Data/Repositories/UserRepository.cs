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
            string sql = "[dbo].[evolUX_GET_USER]";
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
            string sql = "[dbo].[evolUX_GET_USER]";
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
            string sql = @"[dbo].[evolUX_GET_USER_PROFILES]";

            var parameters = new DynamicParameters();
            parameters.Add("UserName", username, DbType.String);
            using (var connection = _context.CreateConnectionEvolFlow())
            {
                roleList = (List<RolesModel>)await connection.QueryAsync<RolesModel>(sql, parameters,
                    commandType: CommandType.StoredProcedure);
                return roleList;
            }
        }

        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserRefreshToken(string username, string refreshToken)
        {
            var query = "UPDATE [dbo].[USERS] SET RefreshToken = @RefreshToken WHERE UserName = @UserName";
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
            var query = "UPDATE [dbo].[USERS] SET RefreshToken = @RefreshToken, RefreshTokenExpiryTime = @RefreshTokenExpiryTime WHERE UserName = @UserName";
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
            var query = "UPDATE [dbo].[USERS] SET RefreshToken = NULL WHERE UserName = @UserName";
            var parameters = new DynamicParameters();
            parameters.Add("UserName", username, DbType.String);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<Result> ChangeCulture(int userID, string culture)
        {
            var sql = "[dbo].[evolUX_UPDATE_USER]";
            var parameters = new DynamicParameters();
            parameters.Add("UserID", userID, DbType.Int32);
            parameters.Add("Language", culture, DbType.String);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                IEnumerable<Result> results = await connection.QueryAsync<Result>(sql, parameters,
                    commandType: CommandType.StoredProcedure);
                return results.First();
            }
        }
    }
}
