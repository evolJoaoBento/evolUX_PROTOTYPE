using Dapper;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using evolUX.API.Models;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using System.Collections;
using System.Data;

namespace evolUX.API.Areas.evolDP.Services
{
    public class ServiceProvisionService : IServiceProvisionService
    {
        private readonly IWrapperRepository _repository;

        public ServiceProvisionService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Company>> GetServiceCompanies(DataTable serviceCompanyList)
        {
            IEnumerable<Company> result = await _repository.Generic.GetCompanies(null, serviceCompanyList);
            return result;
        }
        public async Task<IEnumerable<Company>> GetServiceCompanies(int serviceCompanyID)
        {
            IEnumerable<Company> result = await _repository.Generic.GetCompanies(serviceCompanyID, null);
            return result;
        }
        public async Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int? serviceCompanyID)
        {
            IEnumerable<ServiceCompanyRestriction> result = await _repository.ServiceProvision.GetServiceCompanyRestrictions(serviceCompanyID);
            return result;
        }
        public async Task SetServiceCompanyRestriction(int serviceCompanyID, int materialTypeID, int materialPosition, int fileSheetsCutoffLevel, bool restrictionMode)
        {
            await SetServiceCompanyRestriction(serviceCompanyID, materialTypeID, materialPosition, fileSheetsCutoffLevel, restrictionMode);
            return;
        }

        public async Task<IEnumerable<ServiceCompanyServiceResume>> GetServiceCompanyConfigsResume(int? serviceCompanyID, int? serviceTypeID, int? serviceID, int? costDate)
        {
            IEnumerable<ServiceCompanyServiceResume> result = await _repository.ServiceProvision.GetServiceCompanyConfigsResume(serviceCompanyID, serviceTypeID, serviceID, costDate);
            return result;
        }
        public async Task<IEnumerable<int>> GetServiceCompanyList(int? serviceCompanyID, int? serviceTypeID, int? serviceID, int? costDate)
        {
            IEnumerable<ServiceCompanyServiceResume> result = await _repository.ServiceProvision.GetServiceCompanyConfigsResume(serviceCompanyID, serviceTypeID, serviceID, costDate);
            return result.Select(x => x.ServiceCompanyID).Distinct().ToList();
        }
        public async Task<IEnumerable<ServiceCompanyService>> GetServiceCompanyConfigs(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID)
        {
            IEnumerable<ServiceCompanyService> result = await _repository.ServiceProvision.GetServiceCompanyConfigs(serviceCompanyID, costDate, serviceTypeID, serviceID);
            if (result == null)
            {

            }
            return result;
        }
        public async Task SetService(ServiceElement service)
        {
            await _repository.ServiceProvision.SetService(service);
        }

        public async Task<IEnumerable<ServiceElement>> GetServices(int serviceTypeID)
        {
            IEnumerable<ServiceElement> result = await _repository.ServiceProvision.GetServices(serviceTypeID);
            if (result == null)
            {

            }
            return result;
        }
        public async Task SetServiceCompanyConfig(ServiceCompanyService serviceCompanyConfig)
        {
            await _repository.ServiceProvision.SetServiceCompanyConfig(serviceCompanyConfig);
        }
        
        public async Task<IEnumerable<ServiceTypeElement>> GetAvailableServiceTypes()
        {
            return await _repository.ServiceProvision.GetServiceTypes(null);
        }

        public async Task<ServiceTypeViewModel> GetServiceTypes(int? serviceTypeID)
        {
            ServiceTypeViewModel viewModel = new ServiceTypeViewModel();
            viewModel.Types = await _repository.ServiceProvision.GetServiceTypes(serviceTypeID);
            if (viewModel.Types != null)
            {
                foreach (var type in viewModel.Types)
                {
                    var list = await GetServices(type.ServiceTypeID);
                    if (list != null)
                        type.ServicesList = list.ToList();
                }
            }
            return viewModel;
        }
        public async Task SetServiceType(int serviceTypeID, string serviceTypeCode, string serviceTypeDesc)
        {
            await _repository.ServiceProvision.SetServiceType(serviceTypeID, serviceTypeCode, serviceTypeDesc);
        }
        public async Task<IEnumerable<ServiceTaskElement>> GetServiceTasks(int? serviceTaskID)
        {
            IEnumerable<ServiceTaskElement> result = await _repository.ServiceProvision.GetServiceTasks(serviceTaskID);
            if (result != null)
            {
                foreach(ServiceTaskElement st in result) 
                {
                    st.ServiceTypes = await _repository.ServiceProvision.GetServiceTaskServiceTypes(st.ServiceTaskID);
                }
            }
            return result;
        }
        public async Task SetServiceTask(int serviceTaskID, string serviceTaskCode, string serviceTaskDesc, int refServiceTaskID, int complementServiceTaskID, int externalExpeditionMode, string stationExceededDesc)
        {
            await _repository.ServiceProvision.SetServiceTask(serviceTaskID, serviceTaskCode, serviceTaskDesc, refServiceTaskID, complementServiceTaskID, externalExpeditionMode, stationExceededDesc);
            return;
        }

        public async Task<IEnumerable<ExpCodeElement>> GetExpCodes(int serviceTaskID, int expCompanyID, string expCode)
        {
            IEnumerable<ExpCodeElement> result = await _repository.ServiceProvision.GetExpCodes(serviceTaskID, expCompanyID, expCode);
            if (result == null)
            {

            }
            return result;

        }
        public async Task DeleteServiceType(int serviceTaskID, int serviceTypeID)
        {
            await _repository.ServiceProvision.DeleteServiceType(serviceTaskID, serviceTypeID);
            return;

        }
        public async Task AddServiceType(int serviceTaskID, int serviceTypeID)
        {
            await _repository.ServiceProvision.AddServiceType(serviceTaskID, serviceTypeID);
            return;
        }
        public async Task<IEnumerable<ExpCenterElement>> GetExpCenters(string expCode, DataTable serviceCompanyList)
        {
            IEnumerable<ExpCenterElement> result = await _repository.ServiceProvision.GetExpCenters(expCode, serviceCompanyList);
            if (result == null)
            {

            }
            return result;
        }
    }
}
