using evolUX.Models;

namespace evolUX.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<UserModel>> GetAllUsers();
        UserModel GetById(int id);
        public Task UpdateUserRefreshToken(string username, string refreshToken);
        public Task UpdateUserRefreshTokenAndTime(UserModel user);

        public Task DeleteRefreshToken(string username);
    }
}
