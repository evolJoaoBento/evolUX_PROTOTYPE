

using evolUX.API.Areas.Finishing.Models;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface ISessionRepository
    {
        public Task<string> GetProfile(int user);
        public Task<IEnumerable<string>> GetServers(string profile);
        public Task<DataTable> GetServiceCompanies(IEnumerable<string> servers);
    }
}
