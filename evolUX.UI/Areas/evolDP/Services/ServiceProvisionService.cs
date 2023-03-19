using evolUX.API.Models;
using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using evolUX_dev.Areas.evolDP.Models;
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
        public async Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int serviceCompanyID)
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
            await _serviceProvisionTypeRepository.SetServiceCompanyRestriction(serviceCompanyID, materialTypeID, materialPosition, fileSheetsCutoffLevel, restrictionMode);
            return;
        }
        public async Task<IEnumerable<ServiceCompanyService>> GetServiceCompanyConfigs(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID)
        {
            var response = await _serviceProvisionTypeRepository.GetServiceCompanyConfigs(serviceCompanyID, costDate, serviceTypeID, serviceID);
            return response;
        }
        public async Task SetServiceCompanyConfig(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID, double serviceCost, string formula)
        {
            await _serviceProvisionTypeRepository.SetServiceCompanyConfig(serviceCompanyID, costDate, serviceTypeID, serviceID, serviceCost, formula);
            return;
        }
        public async Task<IEnumerable<ServiceElement>> GetServices(int serviceTypeID)
        {
            var response = await _serviceProvisionTypeRepository.GetServices(serviceTypeID);
            return response;
        }
        public async Task<IEnumerable<ServiceTypeElement>> GetAvailableServiceTypes()
        {
            var response = await _serviceProvisionTypeRepository.GetAvailableServiceTypes();
            return response;
        }
        public async Task<ServiceTypeViewModel> GetServiceTypes()
        {
            var response = await _serviceProvisionTypeRepository.GetServiceTypes();
            return response;
        }
        public async Task SetServiceType(int serviceTypeID, string serviceTypeCode, string serviceTypeDesc)
        {
            await _serviceProvisionTypeRepository.SetServiceType(serviceTypeID, serviceTypeCode, serviceTypeDesc);
            return;
        }
        public async Task<IEnumerable<int>> GetServiceCompanyList(int serviceTypeID, int serviceID, int costDate)
        {
            var response = await _serviceProvisionTypeRepository.GetServiceCompanyList(serviceTypeID, serviceID, costDate);
            return response;
        }
        public async Task<IEnumerable<ServiceTask>> GetServiceTasks(int? serviceTaskID)
        {
            var response = await _serviceProvisionTypeRepository.GetServiceTasks(serviceTaskID);
            return response;
        }

    }
}
