namespace evolUX.API.Areas.Core.Repositories.Interfaces
{
    public interface ISidebarRepository
    {
        public Task<List<dynamic>> GetSidebar();

    }
}
