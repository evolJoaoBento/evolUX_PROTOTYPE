using evolUX.API.Models;
using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using evolUX.UI.Exceptions;
using evolUX_dev.Areas.evolDP.Models;
using Flurl.Http;
using NuGet.Protocol;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using System.Net;

namespace evolUX.UI.Areas.evolDP.Services
{
    public class ServiceProvisionService : IServiceProvisionService
    {
        private readonly IServiceProvisionRepository _serviceProvisionRepository;
        public ServiceProvisionService(IServiceProvisionRepository serviceProvisionRepository)
        {
            _serviceProvisionRepository = serviceProvisionRepository;
        }
        public async Task<IEnumerable<Company>> GetServiceCompanies(string servicCompanyList)
        {
            var response = await _serviceProvisionRepository.GetServiceCompanies(servicCompanyList);
            return response;
        }
        public async Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int serviceCompanyID)
        {
            var response = await _serviceProvisionRepository.GetServiceCompanyRestrictions(serviceCompanyID);
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
                response.Configs = await _serviceProvisionRepository.GetServiceCompanyConfigsResume(response.ServiceCompany.ID);
            }
            return response;
        }

        public async Task<ServiceCompanyViewModel> GetServiceCompanyViewModel(int serviceCompanyID, List<ServiceCompanyRestriction> restrictions)
        {
            ServiceCompanyViewModel response = new ServiceCompanyViewModel();
            var companies = await _serviceProvisionRepository.GetServiceCompany(serviceCompanyID);
            if (companies != null && companies.Count() > 0)
            {
                return await GetServiceCompanyViewModel(companies.First(), restrictions);
            }
            return response;
        }

        public async Task<Company> SetServiceCompany(Company serviceCompany)
        {
            var response = await _serviceProvisionRepository.SetServiceCompany(serviceCompany);
            return response;
        }
        public async Task SetServiceCompanyRestrictions(int serviceCompanyID, int materialTypeID, int materialPosition, int fileSheetsCutoffLevel, bool restrictionMode)
        {
            await _serviceProvisionRepository.SetServiceCompanyRestriction(serviceCompanyID, materialTypeID, materialPosition, fileSheetsCutoffLevel, restrictionMode);
            return;
        }
        public async Task<IEnumerable<ServiceCompanyService>> GetServiceCompanyConfigs(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID)
        {
            var response = await _serviceProvisionRepository.GetServiceCompanyConfigs(serviceCompanyID, costDate, serviceTypeID, serviceID);
            return response;
        }
        public async Task SetServiceCompanyConfig(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID, double serviceCost, string formula)
        {
            await _serviceProvisionRepository.SetServiceCompanyConfig(serviceCompanyID, costDate, serviceTypeID, serviceID, serviceCost, formula);
            return;
        }
        public async Task<IEnumerable<ServiceElement>> GetServices(int serviceTypeID)
        {
            var response = await _serviceProvisionRepository.GetServices(serviceTypeID);
            return response;
        }
        public async Task<IEnumerable<ServiceTypeElement>> GetAvailableServiceTypes()
        {
            var response = await _serviceProvisionRepository.GetAvailableServiceTypes();
            return response;
        }
        public async Task<ServiceTypeViewModel> GetServiceTypes()
        {
            var response = await _serviceProvisionRepository.GetServiceTypes();
            return response;
        }
        public async Task SetServiceType(int serviceTypeID, string serviceTypeCode, string serviceTypeDesc)
        {
            await _serviceProvisionRepository.SetServiceType(serviceTypeID, serviceTypeCode, serviceTypeDesc);
            return;
        }
        public async Task<IEnumerable<int>> GetServiceCompanyList(int serviceTypeID, int serviceID, int costDate)
        {
            var response = await _serviceProvisionRepository.GetServiceCompanyList(serviceTypeID, serviceID, costDate);
            return response;
        }
        public async Task<IEnumerable<ServiceTaskElement>> GetServiceTasks(int? serviceTaskID)
        {
            var response = await _serviceProvisionRepository.GetServiceTasks(serviceTaskID);
            return response;
        }
        public async Task SetServiceTask(int serviceTaskID, string serviceTaskCode, string serviceTaskDesc, int refServiceTaskID, int complementServiceTaskID, int externalExpeditionMode, string stationExceededDesc)
        {
            await _serviceProvisionRepository.SetServiceTask(serviceTaskID, serviceTaskCode, serviceTaskDesc, refServiceTaskID, complementServiceTaskID, externalExpeditionMode, stationExceededDesc);
            return;
        }
        public async Task<IEnumerable<ExpCodeElement>> GetExpCodes(int serviceTaskID, int expCompanyID, string expCode)
        {
            var response = await _serviceProvisionRepository.GetExpCodes(serviceTaskID, expCompanyID, expCode);
            return response;
        }
        public async Task DeleteServiceType(int serviceTaskID, int serviceTypeID)
        {
            await _serviceProvisionRepository.DeleteServiceType(serviceTaskID, serviceTypeID);
            return;
        }
        public async Task AddServiceType(int serviceTaskID, int serviceTypeID)
        {
            await _serviceProvisionRepository.AddServiceType(serviceTaskID, serviceTypeID);
            return;
        }
        public async Task<IEnumerable<ExpCenterElement>> GetExpCenters(string expCode, string serviceCompanyList)
        {
            var response = await _serviceProvisionRepository.GetExpCenters(expCode, serviceCompanyList);
            return response;
        }
        public async Task<IEnumerable<ExpeditionZoneElement>> GetExpeditionZones(int expCompanyID)
        {
            var response = await _serviceProvisionRepository.GetExpeditionZones(expCompanyID);
            return response.Zones;
        }
        public async Task SetExpCenter(string expCode, string expCenterCode, string description1, string description2, string description3, int serviceCompanyID, string expeditionZone)
        {
            await SetExpCenter(expCode, expCenterCode, description1, description2, description3, serviceCompanyID, expeditionZone);
            return;
        }
        public async Task<IEnumerable<ServiceCompanyExpCodeConfig>> GetServiceCompanyExpCodeConfigs(string expCode, int serviceCompanyID, string expCenterCode)
        {
            var response = await _serviceProvisionRepository.GetServiceCompanyExpCodeConfigs(expCode, serviceCompanyID, expCenterCode);
            return response;
        }
        public async Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes()
        {
            var response = await _serviceProvisionRepository.GetFulfillMaterialCodes();
            return response;
        }
        public async Task DeleteServiceCompanyExpCodeConfig(string expCode, int serviceCompanyID, string expCenterCode, int expLevel)
        {
            await _serviceProvisionRepository.DeleteServiceCompanyExpCodeConfig(expCode, serviceCompanyID, expCenterCode, expLevel);
            return;
        }
        public async Task SetServiceCompanyExpCodeConfig(string expCode, int serviceCompanyID, string expCenterCode, int expLevel, string fullFillMaterialCode, int docMaxSheets, string barcode)
        {
            await _serviceProvisionRepository.SetServiceCompanyExpCodeConfig(expCode, serviceCompanyID, expCenterCode, expLevel, fullFillMaterialCode, docMaxSheets, barcode);
            return;
        }
    }
}
