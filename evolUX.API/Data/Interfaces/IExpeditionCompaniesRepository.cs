

namespace evolUX.API.Data.Interfaces
{
    public interface IExpeditionCompaniesRepository
    {
        public Task<List<dynamic>> GetExpeditionCompanies();

        public Task<List<dynamic>> GetExpeditionCompanyConfigs(dynamic data);

        public Task<List<dynamic>> GetExpeditionCompanyConfigCharacteristics(dynamic data);
    }
}
