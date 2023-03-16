using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

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
        public Task<ServiceTypeViewModel> GetServiceTypes();
        public Task SetServiceType(int serviceTypeID, string serviceTypeCode, string serviceTypeDesc);
        public Task<IEnumerable<int>> GetServiceCompanyList(int serviceTypeID, int serviceID, int costDate);
        public Task<IEnumerable<ServiceTask>> GetServiceTasks(int? serviceTaskID);
        //public Task<ExpCompanyViewModel> GetExpCompanyViewModel(int expCompanyID, List<ExpCompanyType> types);
        //public Task<ExpCompanyViewModel> GetExpCompanyViewModel(Company expCompany, List<ExpCompanyType> types);
        //public Task<ExpeditionTypeViewModel> GetExpeditionTypes(int? expeditionType, string expCompanyList);
        //public Task<IEnumerable<ExpCompanyType>> GetExpCompanyTypes(int? expeditionType, int? expCompanyID);
        //public Task SetExpCompanyType(int expeditionType, int expCompanyID, bool registMode, bool separationMode, bool barcodeRegistMode);
        //public Task<IEnumerable<ExpeditionRegistElement>> GetExpeditionRegistIDs(int expCompanyID);
        //public Task SetExpeditionRegistID(ExpeditionRegistElement expRegist);
        //public Task<IEnumerable<ExpContractElement>> GetExpContracts(int expCompanyID);
        //public Task SetExpContract(ExpContractElement expContract);
        //public Task SetExpCompanyConfig(ExpCompanyConfig expCompanyConfig);
        //public Task<IEnumerable<ExpCompanyConfigResume>> GetExpCompanyConfigsResume(int expCompanyID);
        //public Task NewExpCompanyConfig(int expCompanyID, int startDate);
    }
}
