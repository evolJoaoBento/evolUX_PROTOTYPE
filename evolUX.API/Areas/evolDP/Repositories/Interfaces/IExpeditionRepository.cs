using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IExpeditionRepository
    {
        public Task<IEnumerable<ExpeditionTypeElement>> GetExpeditionTypes(int? expeditionType);
        public Task<IEnumerable<ExpCompanyType>> GetExpCompanyTypes(int? expeditionType, int? expCompanyID, DataTable? expCompanyList);
        public Task SetExpCompanyType(int expeditionType, int expCompanyID, bool registMode, bool separationMode, bool barcodeRegistMode); 
        public Task<IEnumerable<ExpeditionZoneElement>> GetExpeditionZones(int? expeditionZone);
        public Task<IEnumerable<ExpCompanyZone>> GetExpCompanyZones(int? expeditionZone, int? expCompanyID, DataTable? expCompanyList);
        public Task<IEnumerable<ExpeditionRegistElement>> GetExpeditionRegistIDs(int expCompanyID);
        public Task<int> SetExpeditionRegistID(ExpeditionRegistElement expRegist);
        public Task<IEnumerable<ExpContractElement>> GetExpContracts(int expCompanyID);
        public Task<int> SetExpContract(ExpContractElement expContract);

        public Task<IEnumerable<ExpCompanyServiceTask>> GetExpCompanyServiceTask(string expCode);

        public Task<IEnumerable<ExpCompanyConfig>> GetExpCompanyConfigs(int expCompanyID, int expeditionType, int expeditionZone);
        public Task SetExpCompanyConfig(ExpCompanyConfig expCompanyConfig);
    }
}
