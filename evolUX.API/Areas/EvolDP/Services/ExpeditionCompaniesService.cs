using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;

namespace evolUX.API.Areas.evolDP.Services
{
    public class ExpeditionCompaniesService : IExpeditionCompaniesService
    {
        private readonly IWrapperRepository _repository;

        public ExpeditionCompaniesService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<dynamic>> GetExpeditionCompanies()
        {
            var expeditionCompaniesList = await _repository.ExpeditionCompanies.GetExpeditionCompanies();
            if (expeditionCompaniesList == null)
            {

            }
            return expeditionCompaniesList;
        }        
        public async Task<List<dynamic>> GetExpeditionCompanyConfigs(dynamic data)
        {
            var expeditionCompanyConfigsList = await _repository.ExpeditionCompanies.GetExpeditionCompanyConfigs(data);
            if (expeditionCompanyConfigsList == null)
            {

            }
            return expeditionCompanyConfigsList;
        }

        public async Task<List<dynamic>> GetExpeditionCompanyConfigCharacteristics(dynamic data)
        {
            var envelopeMediaGroupList = await _repository.ExpeditionCompanies.GetExpeditionCompanyConfigCharacteristics(data);
            if (envelopeMediaGroupList == null)
            {

            }
            return envelopeMediaGroupList;
        }
    }
}
