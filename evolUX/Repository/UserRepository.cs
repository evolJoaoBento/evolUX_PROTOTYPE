using evolUX.Context;
using evolUX.Interfaces;
using evolUX.Models;
using Dapper;
using System.Data;

namespace evolUX.Repository
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
            string sql = "evolUX_GET_USER";
            using (var connection = _context.CreateConnectionEvolFlow())
            {
                userList = (List<UserModel>)await connection.QueryAsync<UserModel>(sql,
                    commandType: CommandType.StoredProcedure);
                return userList;
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

    }
}
