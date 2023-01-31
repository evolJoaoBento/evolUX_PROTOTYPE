using evolUX.API.Areas.Core.Services.Interfaces;
using Shared.Models.Areas.Finishing;
using Shared.ViewModels.Areas.Finishing;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Models.Areas.Core;
using evolUX.API.Areas.Core.Repositories.Interfaces;

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

        public async Task<Dictionary<string, string>> GetSessionVariables([FromQuery] int User)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            IEnumerable<int> profiles = await _repository.Session.GetProfile(User);
            result.Add("evolUX/Profiles", JsonConvert.SerializeObject(profiles));
            IEnumerable<string> servers = await _repository.Session.GetServers(profiles);
            IEnumerable<SideBarAction> sideBarActions = await _repository.Session.GetSideBarActions(profiles);
            result.Add("evolUX/SideBarActions", JsonConvert.SerializeObject(sideBarActions));

            //evolDP
            if (_repository.Session.HasEvolDP())
            {
                DataTable serviceCompanies = await _repository.Session.GetCompanies(servers, "SERVICE");
                result.Add("evolDP/ServiceCompanies", JsonConvert.SerializeObject(serviceCompanies));
                DataTable expeditionCompanies = await _repository.Session.GetCompanies(servers, "EXPEDITION");
                result.Add("evolDP/ExpeditionCompanies", JsonConvert.SerializeObject(expeditionCompanies));
                DataTable companyBusiness = await _repository.Session.GetCompanyBusinness(servers, "SERVICE");
                result.Add("evolDP/CompanyBusiness", JsonConvert.SerializeObject(companyBusiness));
            }
            return result;
        }
    }
}
