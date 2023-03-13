using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IServiceProvisionRepository
    {
        public Task<IEnumerable<ServiceTask>> GetServiceTasks(int? serviceTaskID);
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
    }
}
