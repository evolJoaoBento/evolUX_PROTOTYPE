using evolUX.API.Areas.Core.ViewModels;

namespace evolUX.API.Data.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<UserModel>> GetAllUsers();

        public Task<UserModel> GetUserByUsername(string username);
        UserModel GetById(int id);

        public Task<List<RolesModel>> GetRolesByUsername(string username);
        public Task UpdateUserRefreshToken(string username, string refreshToken);
        public Task UpdateUserRefreshTokenAndTime(UserModel user);

        public Task DeleteRefreshToken(string username);

        public Task ChangeCulture(string culture);
    }
}
