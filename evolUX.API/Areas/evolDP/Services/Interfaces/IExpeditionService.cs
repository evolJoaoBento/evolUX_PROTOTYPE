using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Services.Interfaces
{
    public interface IExpeditionService
    {
        public Task<ExpeditionTypeViewModel> GetExpeditionCompanies(DataTable expCompanyList);
        public Task<ExpeditionTypeViewModel> GetExpeditionTypes(int? expeditionType, DataTable? expCompanyList);
        public Task<IEnumerable<ExpCompanyType>> GetExpCompanyTypes(int? expeditionType, int? expCompanyID);
        public Task<Result> SetExpCompanyType(int expeditionType, int expCompanyID, bool registMode, bool separationMode, bool barcodeRegistMode);
        public Task<ExpeditionZoneViewModel> GetExpeditionZones(int? expeditionZone, DataTable? expCompanyList);
        public Task<IEnumerable<Company>> GetExpeditionCompanies(int? expCompanyID, DataTable? expCompanyList);
        public Task<IEnumerable<ExpeditionRegistElement>> GetExpeditionRegistIDs(int expCompanyID);
        public Task SetExpeditionRegistID(ExpeditionRegistElement expRegist);
        public Task<IEnumerable<ExpContractElement>> GetExpContracts(int expCompanyID);
        public Task SetExpContract(ExpContractElement expContract);
        public Task<IEnumerable<ExpCompanyConfig>> GetExpCompanyConfigs(int expCompanyID, int expeditionType, int expeditionZone);
        public Task SetExpCompanyConfig(ExpCompanyConfig expCompanyConfig);
    }
}
