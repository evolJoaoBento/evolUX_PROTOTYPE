using evolUX.Models;

namespace evolUX.Interfaces
{
    public interface ISidebarRepository
    {
        public Task<List<dynamic>> GetSidebar();

    }
}
