using evolUX.Context;
using evolUX.Interfaces;
using evolUX.Models;
using Dapper;

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
            string sql = "SELECT UserId AS [ID], UserName AS [Username], Password FROM USERS";
            using (var connection = _context.CreateConnectionEvolFlow())
            {
                userList = (List<UserModel>)await connection.QueryAsync<UserModel>(sql);
                return userList;
            }
        }

        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
