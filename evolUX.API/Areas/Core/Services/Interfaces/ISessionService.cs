using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Core.Services.Interfaces
{
    public interface ISessionService
    {
        public Task<IEnumerable<int>> GetProfile(int user);
        public Task<IEnumerable<string>> GetServers(IEnumerable<int> profiles);
        public Task<DataTable> GetServiceCompanies(IEnumerable<string> servers);
    }
}
