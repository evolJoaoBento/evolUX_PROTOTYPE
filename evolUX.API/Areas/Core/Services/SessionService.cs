using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.Finishing.Models;
using evolUX.API.Areas.Finishing.ViewModels;
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

        public async Task<string> GetProfile(int user)
        {
            string profiles = await _repository.Session.GetProfile(user);
            if (profiles == null)
            {

            }
            return profiles;
        }

        public async Task<IEnumerable<string>> GetServers(string profile)
        {
            IEnumerable<string> servers = await _repository.Session.GetServers(profile);
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
