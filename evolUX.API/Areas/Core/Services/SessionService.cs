using evolUX.API.Areas.Core.Services.Interfaces;
using SharedModels.Models.Areas.Finishing;
using SharedModels.ViewModels.Areas.Finishing;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Areas.Core.Services
{
    public class SessionService : ISessionService
    {
        private readonly IWrapperRepository _repository;
        public SessionService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<int>> GetProfile(int user)
        {
            IEnumerable<int> profiles = await _repository.Session.GetProfile(user);
            if (profiles == null)
            {

            }
            return profiles;
        }

        public async Task<IEnumerable<string>> GetServers(IEnumerable<int> profiles)
        {
            IEnumerable<string> servers = await _repository.Session.GetServers(profiles);
            if (servers == null)
            {

            }
            return servers;
        }

        public async Task<DataTable> GetServiceCompanies(IEnumerable<string> servers)
        {
            DataTable serviceCompanies = await _repository.Session.GetServiceCompanies(servers);
            if (serviceCompanies == null)
            {

            }
            return serviceCompanies;
        }
    }
}
