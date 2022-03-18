using evolUX.Models;

namespace evolUX.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<UserModel>> GetAllUsers();
        UserModel GetById(int id);
    }
}
