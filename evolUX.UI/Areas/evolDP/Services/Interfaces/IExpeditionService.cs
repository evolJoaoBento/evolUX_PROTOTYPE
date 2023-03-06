using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Services.Interfaces
{
    public interface IExpeditionService
    {
        public Task<ExpeditionTypeViewModel> GetExpeditionCompanies(string expCompanyList);
        public Task<Company> SetExpCompany(Company expCompany);
        public Task<ExpCompanyViewModel> GetExpCompanyViewModel(int expCompanyID, List<ExpCompanyType> types);
        public Task<ExpCompanyViewModel> GetExpCompanyViewModel(Company expCompany, List<ExpCompanyType> types);
        public Task<ExpeditionTypeViewModel> GetExpeditionTypes(int? expeditionType, string expCompanyList);
        public Task<IEnumerable<ExpCompanyType>> GetExpCompanyTypes(int? expeditionType, int? expCompanyID);
        public Task SetExpCompanyType(int expeditionType, int expCompanyID, bool registMode, bool separationMode, bool barcodeRegistMode);
        public Task<ExpeditionZoneViewModel> GetExpeditionZones(int? expeditionZone, string expCompanyList);
        public Task<IEnumerable<ExpeditionRegistElement>> GetExpeditionRegistIDs(int expCompanyID);
        public Task SetExpeditionRegistID(ExpeditionRegistElement expRegist);
        public Task<IEnumerable<ExpContractElement>> GetExpContracts(int expCompanyID);
        public Task SetExpContract(ExpContractElement expContract);
        public Task<IEnumerable<ExpCompanyConfig>> GetExpCompanyConfigs(int expCompanyID, int startDate, int expeditionType, int expeditionZone);
        public Task SetExpCompanyConfig(ExpCompanyConfig expCompanyConfig);
        public Task<IEnumerable<ExpCompanyConfigResume>> GetExpCompanyConfigsResume(int expCompanyID);
        public Task NewExpCompanyConfig(int expCompanyID, int startDate);
    }
}
