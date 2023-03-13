namespace evolUX.UI.Areas.Core.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task ChangeCulture(int userID, string culture);
    }
}
