using evolUX.API.Areas.Core.ViewModels;
using Shared.Models.General;

namespace evolUX.API.Areas.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> LoginUserCredentials(LoginRequest model);
        Task<UserModel> LoginUserWindows(LoginRequest model);
        Task UpdateUserRefreshTokenAndTime(UserModel user);
        Task UpdateUserRefreshToken(string username, string refreshToken);
        Task<UserModel> GetUserByUsername(string username);
        Task DeleteRefreshToken(string username);
        public Task<Result> ChangeCulture(int userID, string culture);
    }
}
