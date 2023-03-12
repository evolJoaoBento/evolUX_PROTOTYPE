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
        public Task<ServiceTypeViewModel> GetServiceTypes(int? serviceTypeID);
        //public Task<ExpeditionTypeViewModel> GetExpeditionTypes(int? expeditionType, DataTable? expCompanyList);
        //public Task<IEnumerable<ExpCompanyType>> GetExpCompanyTypes(int? expeditionType, int? expCompanyID);
        //public Task<Result> SetExpCompanyType(int expeditionType, int expCompanyID, bool registMode, bool separationMode, bool barcodeRegistMode);
        //public Task<IEnumerable<Company>> GetExpeditionCompanies(int? expCompanyID, DataTable? expCompanyList);
        //public Task<IEnumerable<ExpeditionRegistElement>> GetExpeditionRegistIDs(int expCompanyID);
        //public Task SetExpeditionRegistID(ExpeditionRegistElement expRegist);
        //public Task<IEnumerable<ExpContractElement>> GetExpContracts(int expCompanyID);
        //public Task SetExpContract(ExpContractElement expContract);
        //public Task<IEnumerable<ExpCompanyConfigResume>> GetExpCompanyConfigsResume(int expCompanyID);
        //public Task NewExpCompanyConfig(int expCompanyID, int startDate);
    }
}
