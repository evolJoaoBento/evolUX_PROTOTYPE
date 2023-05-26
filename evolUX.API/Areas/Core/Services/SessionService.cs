using evolUX.API.Areas.Core.Services.Interfaces;
using Shared.Models.Areas.Finishing;
using Shared.ViewModels.Areas.Finishing;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Models.Areas.Core;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using Shared.Models.Areas.evolDP;
using Microsoft.IdentityModel.Tokens;

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
            if (profiles.IsNullOrEmpty())
            {
                throw new UnauthorizedAccessException("No Profiles Found");
            }
            result.Add("evolUX/Profiles", JsonConvert.SerializeObject(profiles));
            IEnumerable<string> servers = await _repository.Session.GetServers(profiles);
            IEnumerable<SideBarAction> sideBarActions = await _repository.Session.GetSideBarActions(profiles);
            result.Add("evolUX/SideBarActions", JsonConvert.SerializeObject(sideBarActions));
            IEnumerable<string> permissions = await _repository.Session.GetPermissions(profiles);
            result.Add("evolUX/Permissions", JsonConvert.SerializeObject(permissions));
            //evolDP
            if (_repository.Session.HasEvolDP())
            {
                //IgnoreFilePrinterSpecsPermition;
                //PrintFilePermition;

                DataTable evolDP_DESCRIPTION = await _repository.Session.evolDP_DESCRIPTION();
                result.Add("evolDP/evolDP_DESCRIPTION", JsonConvert.SerializeObject(evolDP_DESCRIPTION));
                DataTable companies= await _repository.Session.GetCompanies(servers, "");
                result.Add("evolDP/Companies", JsonConvert.SerializeObject(companies));
                DataTable companyBusiness = await _repository.Session.GetCompanyBusiness(servers, "");
                result.Add("evolDP/CompanyBusiness", JsonConvert.SerializeObject(companyBusiness));


                DataTable serviceCompanies = await _repository.Session.GetCompanies(servers, "SERVICE");
                result.Add("evolDP/ServiceCompanies", JsonConvert.SerializeObject(serviceCompanies));
                DataTable serviceCompanyBusiness = await _repository.Session.GetCompanyBusiness(servers, "SERVICE");
                result.Add("evolDP/ServiceCompanyBusiness", JsonConvert.SerializeObject(serviceCompanyBusiness));

                DataTable expeditionCompanies = await _repository.Session.GetCompanies(servers, "EXPEDITION");
                result.Add("evolDP/ExpeditionCompanies", JsonConvert.SerializeObject(expeditionCompanies));

                GenericOptionList SuportTypeList = await _repository.DocCode.GetSuporTypeOptionList();
                if (SuportTypeList != null)
                {
                    List<GenericOptionValue> optionList = new List<GenericOptionValue>();
                    if (SuportTypeList.List != null && SuportTypeList.List.Count() > 0)
                    {
                        List<string> options = SuportTypeList.List.Select(x => x.GroupCode).Distinct().ToList();
                        foreach (string option in options)
                        {
                            optionList.Add(new GenericOptionValue()
                            {
                                ID = SuportTypeList.List.Where(x => x.GroupCode == option && x.ID != 0).Min(x => x.ID),
                                Code = option,
                                GroupCode = option
                            });
                        }
                    }
                    SuportTypeList.OptionList = optionList;
                }
                result.Add("evolDP/SuportTypeList", JsonConvert.SerializeObject(SuportTypeList));
            }
            return result;
        }
    }
}
