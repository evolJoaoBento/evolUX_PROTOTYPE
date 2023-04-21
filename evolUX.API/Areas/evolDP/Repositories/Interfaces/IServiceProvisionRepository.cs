using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IServiceProvisionRepository
    {
        public Task<IEnumerable<ServiceTaskElement>> GetServiceTasks(int? serviceTaskID);
        public Task<IEnumerable<ServiceTypeElement>> GetServiceTaskServiceTypes(int serviceTaskID);
        public Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int? serviceCompanyID);
        public Task SetServiceCompanyRestriction(int serviceCompanyID, int materialTypeID, int materialPosition, int fileSheetsCutoffLevel, bool restrictionMode);
        public Task<IEnumerable<ServiceCompanyServiceResume>> GetServiceCompanyConfigsResume(int? serviceCompanyID, int? serviceTypeID, int? serviceID, int? costDate);
        public Task<IEnumerable<ServiceCompanyService>> GetServiceCompanyConfigs(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID);
        public Task SetServiceCompanyConfig(ServiceCompanyService serviceCompanyConfig);
        public Task<IEnumerable<ServiceElement>> GetServices(int serviceTypeID);
        public Task SetService(ServiceElement service);
        public Task<IEnumerable<ServiceTypeElement>> GetServiceTypes(int? serviceTypeID);
        public Task SetServiceType(int serviceTypeID, string serviceTypeCode, string serviceTypeDesc);
        public Task SetServiceTask(int serviceTaskID, string serviceTaskCode, string serviceTaskDesc, int refServiceTaskID, int complementServiceTaskID, int? externalExpeditionMode, string stationExceededDesc);
        public Task<IEnumerable<ExpCodeElement>> GetExpCodes(int serviceTaskID, int expCompanyID, string expCode, DataTable expCompanyList);
        public Task DeleteServiceType(int serviceTaskID, int serviceTypeID);
        public Task AddServiceType(int serviceTaskID, int serviceTypeID);
        public Task<IEnumerable<ExpCenterElement>> GetExpCenters(string expCode, DataTable serviceCompanyList);
        public Task<IEnumerable<ServiceCompanyExpCodeElement>> GetServiceCompanyExpCodes(int serviceCompanyID, DataTable expCompanyList);
        public Task SetExpCenter(string expCode, string expCenterCode, string description1, string description2, string description3, int serviceCompanyID, string expeditionZone);
        public Task<IEnumerable<ServiceCompanyExpCodeConfig>> GetServiceCompanyExpCodeConfigs(string expCode, int serviceCompanyID, string expCenterCode);
        public Task SetServiceCompanyExpCodeConfig(string expCode, int serviceCompanyID, string expCenterCode, int expLevel, string fullFillMaterialCode, int docMaxSheets, string barcode);
        public Task DeleteServiceCompanyExpCodeConfig(string expCode, int serviceCompanyID, string expCenterCode, int expLevel);
    }
}
