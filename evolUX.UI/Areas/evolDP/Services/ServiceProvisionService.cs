using evolUX.API.Models;
using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Services
{
    public class ServiceProvisionService : IServiceProvisionService
    {
        private readonly IServiceProvisionRepository _serviceProvisionTypeRepository;
        public ServiceProvisionService(IServiceProvisionRepository serviceProvisionTypeRepository)
        {
            _serviceProvisionTypeRepository = serviceProvisionTypeRepository;
        }
        public async Task<IEnumerable<Company>> GetServiceCompanies(string servicCompanyList)
        {
            var response = await _serviceProvisionTypeRepository.GetServiceCompanies(servicCompanyList);
            return response;
        }
        public async Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int? serviceCompanyID)
        {
            var response = await _serviceProvisionTypeRepository.GetServiceCompanyRestrictions(serviceCompanyID);
            return response;
        }        

        public async Task<ServiceCompanyViewModel> GetServiceCompanyViewModel(Company serviceCompany, List<ServiceCompanyRestriction> restrictions)
        {
            ServiceCompanyViewModel response = new ServiceCompanyViewModel();
            if (serviceCompany != null)
            {
                response.ServiceCompany = serviceCompany;
                if (restrictions == null)
                    restrictions = (await GetServiceCompanyRestrictions(response.ServiceCompany.ID))?.ToList();
                response.Restrictions = restrictions;
                response.Configs = await _serviceProvisionTypeRepository.GetServiceCompanyConfigsResume(response.ServiceCompany.ID);
            }
            return response;
        }

        public async Task<ServiceCompanyViewModel> GetServiceCompanyViewModel(int serviceCompanyID, List<ServiceCompanyRestriction> restrictions)
        {
            ServiceCompanyViewModel response = new ServiceCompanyViewModel();
            var companies = await _serviceProvisionTypeRepository.GetServiceCompany(serviceCompanyID);
            if (companies  != null && companies.Count() > 0)
            {
                return await GetServiceCompanyViewModel(companies.First(), restrictions);
            }
            return response;
        }

        public async Task<Company> SetServiceCompany(Company serviceCompany)
        {
            var response = await _serviceProvisionTypeRepository.SetServiceCompany(serviceCompany);
            return response;
        }
        public async Task SetServiceCompanyRestrictions(int serviceCompanyID, int materialTypeID, int materialPosition, int fileSheetsCutoffLevel, bool restrictionMode)
        {
            await _serviceProvisionTypeRepository.SetServiceCompanyRestrictions(serviceCompanyID, materialTypeID, materialPosition, fileSheetsCutoffLevel, restrictionMode);
            return;
        }
        public async Task<IEnumerable<ServiceCompanyService>> GetServiceCompanyConfigs(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID)
        {
            var response = await _serviceProvisionTypeRepository.GetServiceCompanyConfigs(serviceCompanyID, costDate, serviceTypeID, serviceID);
            return response;
        }

    }
}
