namespace evolUX.API.Areas.evolDP.Services.Interfaces
{
    public interface IExpeditionService
    {
        public Task<List<dynamic>> GetExpeditionTypes();
        public Task<List<dynamic>> GetExpeditionZones();
        public Task<List<dynamic>> GetExpeditionCompanies();
        public Task<List<dynamic>> GetExpeditionCompanyConfigs(dynamic data);
        public Task<List<dynamic>> GetExpeditionCompanyConfigCharacteristics(dynamic data);
    }
}
