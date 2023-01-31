using evolUX.API.Areas.Core.ViewModels;
using Shared.Models.General;

namespace evolUX.API.Areas.Core.Repositories.Interfaces
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

        public Task<Result> ChangeCulture(int userID, string culture);
    }
}
