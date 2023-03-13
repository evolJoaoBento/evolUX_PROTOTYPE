using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using evolUX_dev.Areas.evolDP.Models;
using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;
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
        public async Task<ExpCompanyViewModel> GetExpCompanyViewModel(Company expCompany, List<ExpCompanyType> expTypes)
        {
            ExpCompanyViewModel response = new ExpCompanyViewModel();
            if (expCompany != null)
            {
                response.ExpCompany = expCompany;
                if (expTypes == null)
                    expTypes = (await _expeditionTypeRepository.GetExpCompanyTypes(null, response.ExpCompany.ID))?.ToList();
                response.ExpTypes = expTypes;
                response.Configs = await GetExpCompanyConfigsResume(response.ExpCompany.ID);
                ExpeditionTypeViewModel types = await GetExpeditionTypes(0, "");
                response.Types = types.Types.ToList();
            }
            return response;
        }

        public async Task<ExpCompanyViewModel> GetExpCompanyViewModel(int expCompanyID, List<ExpCompanyType> expTypes)
        {
            ExpCompanyViewModel response = new ExpCompanyViewModel();
            ExpeditionTypeViewModel companies = await _expeditionTypeRepository.GetExpeditionCompanies(expCompanyID);
            if (companies.ExpCompanies != null && companies.ExpCompanies.Count() > 0)
            {
                return await GetExpCompanyViewModel(companies.ExpCompanies.First(),expTypes);
            }
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
        public async Task<IEnumerable<ExpCompanyType>> GetExpCompanyTypes(int? expeditionType, int? expCompanyID)
        {
            var response = await _expeditionTypeRepository.GetExpCompanyTypes(expeditionType, expCompanyID);
            return response;
        }
        public async Task SetExpCompanyType(int expeditionType, int expCompanyID, bool registMode, bool separationMode, bool barcodeRegistMode)
        {
            await _expeditionTypeRepository.SetExpCompanyType(expeditionType, expCompanyID, registMode, separationMode, barcodeRegistMode);
            return;

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
        public async Task<IEnumerable<ExpCompanyConfig>> GetExpCompanyConfigs(int expCompanyID, int startDate, int expeditionType, int expeditionZone)
        {
            var response = await _expeditionTypeRepository.GetExpCompanyConfigs(expCompanyID, startDate, expeditionType, expeditionZone);
            return response;
        }
        public async Task SetExpCompanyConfig(ExpCompanyConfig expCompanyConfig)
        {
            await _expeditionTypeRepository.SetExpCompanyConfig(expCompanyConfig);
            return;
        }
        public async Task NewExpCompanyConfig(int expCompanyID, int startDate)
        {
            await _expeditionTypeRepository.NewExpCompanyConfig(expCompanyID, startDate);
            return;
        }
        public async Task<IEnumerable<ExpCompanyConfigResume>> GetExpCompanyConfigsResume(int expCompanyID)
        {
            var response = await _expeditionTypeRepository.GetExpCompanyConfigsResume(expCompanyID);
            return response;
        }
    }
}
