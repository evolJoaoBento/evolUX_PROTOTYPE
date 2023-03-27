using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.UI.Areas.evolDP.Services.Interfaces
{
    public interface IServiceProvisionService
    {
        public Task<IEnumerable<Company>> GetServiceCompanies(string serviceCompanyList);
        public Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int serviceCompanyID);
        public Task<ServiceCompanyViewModel> GetServiceCompanyViewModel(Company serviceCompany, List<ServiceCompanyRestriction> restrictions);
        public Task<ServiceCompanyViewModel> GetServiceCompanyViewModel(int serviceCompanyID, List<ServiceCompanyRestriction> restrictions);
        public Task<Company> SetServiceCompany(Company expCompany);
        public Task SetServiceCompanyRestrictions(int serviceCompanyID, int materialTypeID, int materialPosition, int fileSheetsCutoffLevel, bool restrictionMode);
        public Task<IEnumerable<ServiceCompanyService>> GetServiceCompanyConfigs(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID);
        public Task SetServiceCompanyConfig(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID, double serviceCost, string formula);
        public Task<IEnumerable<ServiceElement>> GetServices(int serviceTypeID);
        public Task<IEnumerable<ServiceTypeElement>> GetAvailableServiceTypes();
        public Task<ServiceTypeViewModel> GetServiceTypes();
        public Task SetServiceType(int serviceTypeID, string serviceTypeCode, string serviceTypeDesc);
        public Task<IEnumerable<int>> GetServiceCompanyList(int serviceTypeID, int serviceID, int costDate);
        public Task<IEnumerable<ServiceTaskElement>> GetServiceTasks(int? serviceTaskID);
        public Task SetServiceTask(int serviceTaskID, string serviceTaskCode, string serviceTaskDesc, int refServiceTaskID, int complementServiceTaskID, int externalExpeditionMode, string stationExceededDesc);
        public Task<IEnumerable<ExpCodeElement>> GetExpCodes(int serviceTaskID, int expCompanyID, string expCode);
        public Task<IEnumerable<ExpCodeElement>> GetExpCodes(string expCompanyList);
        public Task DeleteServiceType(int serviceTaskID, int serviceTypeID);
        public Task AddServiceType(int serviceTaskID, int serviceTypeID);
        public Task<IEnumerable<ExpCenterElement>> GetExpCenters(string expCode, string serviceCompanyList);
        public Task<IEnumerable<ExpeditionZoneElement>> GetExpeditionZones(int expCompanyID);
        public Task SetExpCenter(string expCode, string expCenterCode, string description1, string description2, string description3, int serviceCompanyID, string expeditionZone);
        public Task<IEnumerable<ServiceCompanyExpCodeConfig>> GetServiceCompanyExpCodeConfigs(string expCode, int serviceCompanyID, string expCenterCode);
        public Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes();
        public Task SetServiceCompanyExpCodeConfig(string expCode, int serviceCompanyID, string expCenterCode, int expLevel, string fullFillMaterialCode, int docMaxSheets, string barcode);
        public Task DeleteServiceCompanyExpCodeConfig(string expCode, int serviceCompanyID, string expCenterCode, int expLevel);
        public Task<IEnumerable<ServiceCompanyExpCodeElement>> GetServiceCompanyExpCodes(int serviceCompanyID, string expCompanyList);
    }
}
