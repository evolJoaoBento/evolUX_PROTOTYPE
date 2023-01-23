using evolUX.API.Areas.Core.ViewModels;
using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Data.Interfaces;
using System.Security.Authentication;
using System.Security.Cryptography;

namespace evolUX.API.Areas.Core.Services
{
    public class UserService : IUserService
    {
        
        private readonly IWrapperRepository _repository;
        private readonly IPasswordHasherService _passwordHasherService;
        public UserService(IWrapperRepository wrapperRepository, IPasswordHasherService passwordHasherService)
        {
            _repository = wrapperRepository;
            _passwordHasherService = passwordHasherService;
        }
        public async Task<UserModel> LoginUserCredentials(LoginRequest model)
        {
            var user = await _repository.User.GetUserByUsername(model.Username);
            if(user == null)
            {
                throw new AuthenticationException();
            }

            //var isValid = _passwordHasherService.Check(user.Password, model.Password);
            //.Verified
            var isValid = true;
            if (!isValid)
            {
                throw new AuthenticationException();
            }
            var roles = _repository.User.GetRolesByUsername(user.Username);
            user.Roles = roles.Result;
            return user;
        }

        public async Task<UserModel> LoginUserWindows(LoginRequest model)
        {
            var user = await _repository.User.GetUserByUsername(model.Username);
            if(user == null)
            {
                return null;
            }
            var roles = _repository.User.GetRolesByUsername(user.Username);
            user.Roles = roles.Result;
            return user;
        }

        public async Task UpdateUserRefreshTokenAndTime(UserModel user)
        {
            await _repository.User.UpdateUserRefreshTokenAndTime(user);
        }        
        
        public async Task UpdateUserRefreshToken(string username, string refreshToken)
        {
            await _repository.User.UpdateUserRefreshToken(username, refreshToken);
        }

        public async Task<UserModel> GetUserByUsername(string username)
        {
            return await _repository.User.GetUserByUsername(username);
        }

        public async Task DeleteRefreshToken(string username)
        {
            await _repository.User.DeleteRefreshToken(username);
        }

        public async Task ChangeCulture(string culture)
        {
            await _repository.User.ChangeCulture(culture);
        }
    }
}
