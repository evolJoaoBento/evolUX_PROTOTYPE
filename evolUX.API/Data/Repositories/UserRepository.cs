using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using Dapper;
using System.Data;
using evolUX.API.Areas.Core.Models;

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
            var userList = new List<UserModel>();
            string sql = "SELECT UserId AS [Id], UserName AS [Username], RefreshToken, RefreshTokenExpiryTime FROM USERS";
            using (var connection = _context.CreateConnectionEvolFlow())
            {
                userList = (List<UserModel>)await connection.QueryAsync<UserModel>(sql);
                return userList;
            }
        }

        public async Task<UserModel> GetUserByUsername(string username)
        {
            var user = new UserModel();
            string sql = "SELECT UserId AS [Id], UserName AS [Username], Password, RefreshToken, RefreshTokenExpiryTime FROM USERS Where UserName = @USERNAME";
            var parameters = new DynamicParameters();
            parameters.Add("USERNAME", username ,DbType.String);
            using (var connection = _context.CreateConnectionEvolFlow())
            {
                user = await connection.QuerySingleOrDefaultAsync<UserModel>(sql, parameters);
                return user;
            }
        }

        public async Task<List<RolesModel>> GetRolesByUsername(string username)
        {
            var roleList = new List<RolesModel>();
            string sql = @"SELECT P.* 
FROM USERS U
JOIN USER_PROFILES UP ON U.UserID = UP.UserID
JOIN PROFILES P ON UP.ProfileID = P.ProfileID
WHERE U.UserName = @USERNAME";
            var parameters = new DynamicParameters();
            parameters.Add("USERNAME", username, DbType.String);
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
            var query = "UPDATE Users SET RefreshToken = @RefreshToken WHERE UserName = @UserName";
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
            var query = "UPDATE Users SET RefreshToken = @RefreshToken, RefreshTokenExpiryTime = @RefreshTokenExpiryTime WHERE UserName = @UserName";
            var parameters = new DynamicParameters();
            parameters.Add("RefreshToken", user.RefreshToken, DbType.String);
            parameters.Add("RefreshTokenExpiryTime", user.RefreshTokenExpiryTime, DbType.DateTime2);
            parameters.Add("UserName", user.Username, DbType.String);

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


    }
}
