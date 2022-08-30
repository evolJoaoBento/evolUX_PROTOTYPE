using evolUX.API.Areas.Finishing.ViewModels;
using evolUX.API.Areas.Finishing.Models;
using System.Data;

namespace evolUX.API.Areas.Core.Services.Interfaces
{
    public interface ISessionService
    {
        public Task<string> GetProfile(int user);
        public Task<IEnumerable<string>> GetServers(string profile);
        public Task<DataTable> GetServiceCompanies(IEnumerable<string> servers);
    }
}
