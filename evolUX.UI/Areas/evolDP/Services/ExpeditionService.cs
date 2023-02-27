using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using evolUX_dev.Areas.evolDP.Models;
using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Services
{
    public class ExpeditionService : IExpeditionService
    {
        private readonly IExpeditionRepository _expeditionTypeRepository;
        public ExpeditionService(IExpeditionRepository expeditionTypeRepository)
        {
            _expeditionTypeRepository = expeditionTypeRepository;
        }
        public async Task<Company> SetExpCompany(Company expCompany)
        {
            var response = await _expeditionTypeRepository.SetExpCompany(expCompany);
            return response;
        }
        public async Task<ExpeditionTypeViewModel> GetExpeditionCompanies(string expCompanyList)
        {
            var response = await _expeditionTypeRepository.GetExpeditionCompanies(expCompanyList);
            return response;
        }
        public async Task<ExpeditionTypeViewModel> GetExpeditionTypes(int? expeditionType, string expCompanyList)
        {
            var response = await _expeditionTypeRepository.GetExpeditionTypes(expeditionType, expCompanyList);
            return response;
        }
        public async Task<ExpeditionTypeViewModel> GetExpCompanyTypes(int? expeditionType, int? expCompanyID)
        {
            var response = await _expeditionTypeRepository.GetExpCompanyTypes(expeditionType, expCompanyID);
            return response;
        }
        public async Task<ExpeditionTypeViewModel> SetExpCompanyType(int expeditionType, int expCompanyID, bool registMode, bool separationMode, bool barcodeRegistMode, bool returnAll)
        {
            var response = await _expeditionTypeRepository.SetExpCompanyType(expeditionType, expCompanyID, registMode, separationMode, barcodeRegistMode, returnAll);
            return response;

        }
        public async Task<ExpeditionZoneViewModel> GetExpeditionZones(int? expeditionZone, string expCompanyList)
        {
            var response = await _expeditionTypeRepository.GetExpeditionZones(expeditionZone, expCompanyList);
            return response;
        }

        public async Task<IEnumerable<ExpeditionRegistElement>> GetExpeditionRegistIDs(int expCompanyID)
        {
            var response = await _expeditionTypeRepository.GetExpeditionRegistIDs(expCompanyID);
            return response;
        }
        public async Task SetExpeditionRegistID(ExpeditionRegistElement expRegist)
        {
            await _expeditionTypeRepository.SetExpeditionRegistID(expRegist);
            return;
        }
        public async Task<IEnumerable<ExpContractElement>> GetExpContracts(int expCompanyID)
        {
            var response = await _expeditionTypeRepository.GetExpContracts(expCompanyID);
            return response;
        }
        public async Task SetExpContract(ExpContractElement expContract)
        {
            await _expeditionTypeRepository.SetExpContract(expContract);
            return;
        }

    }
}
