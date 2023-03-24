using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Services.Interfaces
{
    public interface IServiceProvisionService
    {
        public Task<IEnumerable<Company>> GetServiceCompanies(DataTable serviceCompanyList);
        public Task<IEnumerable<Company>> GetServiceCompanies(int serviceCompanyID);
        public Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int? serviceCompanyID);
        public Task SetServiceCompanyRestriction(int serviceCompanyID, int materialTypeID, int materialPosition, int fileSheetsCutoffLevel, bool restrictionMode);
        public Task<IEnumerable<ServiceCompanyServiceResume>> GetServiceCompanyConfigsResume(int? serviceCompanyID, int? serviceTypeID, int? serviceID, int? costDate);
        public Task<IEnumerable<ServiceCompanyService>> GetServiceCompanyConfigs(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID);
        public Task SetServiceCompanyConfig(ServiceCompanyService serviceCompanyConfig);
        public Task<IEnumerable<ServiceElement>> GetServices(int serviceTypeID);
        public Task SetService(ServiceElement service);
        public Task<IEnumerable<ServiceTypeElement>> GetAvailableServiceTypes();
        public Task<ServiceTypeViewModel> GetServiceTypes(int? serviceTypeID);
        public Task SetServiceType(int serviceTypeID, string serviceTypeCode, string serviceTypeDesc);
        public Task<IEnumerable<int>> GetServiceCompanyList(int? serviceCompanyID, int? serviceTypeID, int? serviceID, int? costDate);
        public Task<IEnumerable<ServiceTaskElement>> GetServiceTasks(int? serviceTaskID);
        public Task SetServiceTask(int serviceTaskID, string serviceTaskCode, string serviceTaskDesc, int refServiceTaskID, int complementServiceTaskID, int externalExpeditionMode, string stationExceededDesc);
        public Task<IEnumerable<ExpCodeElement>> GetExpCodes(int serviceTaskID, int expCompanyID, string expCode);
        public Task DeleteServiceType(int serviceTaskID, int serviceTypeID);
        public Task AddServiceType(int serviceTaskID, int serviceTypeID);
        public Task<IEnumerable<ExpCenterElement>> GetExpCenters(string expCode, DataTable serviceCompanyList);
    }
}
