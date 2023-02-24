using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;

namespace evolUX.API.Areas.evolDP.Services
{
    public class ExpeditionService : IExpeditionService
    {
        private readonly IWrapperRepository _repository;
        public ExpeditionService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<dynamic>> GetExpeditionTypes()
        {
            var expeditionList = await _repository.Expedition.GetExpeditionTypes();
            if (expeditionList == null)
            {

            }
            return expeditionList;
        }
        public async Task<List<dynamic>> GetExpeditionZones()
        {
            var expeditionList = await _repository.Expedition.GetExpeditionZones();
            if (expeditionList == null)
            {

            }
            return expeditionList;
        }

        public async Task<List<dynamic>> GetExpeditionCompanies()
        {
            var expeditionCompaniesList = await _repository.Expedition.GetExpeditionCompanies();
            if (expeditionCompaniesList == null)
            {

            }
            return expeditionCompaniesList;
        }
        public async Task<List<dynamic>> GetExpeditionCompanyConfigs(dynamic data)
        {
            var expeditionCompanyConfigsList = await _repository.Expedition.GetExpeditionCompanyConfigs(data);
            if (expeditionCompanyConfigsList == null)
            {

            }
            return expeditionCompanyConfigsList;
        }

        public async Task<List<dynamic>> GetExpeditionCompanyConfigCharacteristics(dynamic data)
        {
            var envelopeMediaGroupList = await _repository.Expedition.GetExpeditionCompanyConfigCharacteristics(data);
            if (envelopeMediaGroupList == null)
            {

            }
            return envelopeMediaGroupList;
        }
    }
}
