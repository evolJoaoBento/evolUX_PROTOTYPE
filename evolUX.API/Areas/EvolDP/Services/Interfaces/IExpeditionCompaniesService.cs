﻿namespace evolUX.API.Areas.evolDP.Services.Interfaces
{
    public interface IExpeditionCompaniesService
    {
        public Task<List<dynamic>> GetExpeditionCompanies();
        public Task<List<dynamic>> GetExpeditionCompanyConfigs(dynamic data);
        public Task<List<dynamic>> GetExpeditionCompanyConfigCharacteristics(dynamic data);
    }
}
