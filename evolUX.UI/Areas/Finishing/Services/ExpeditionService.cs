using evolUX.UI.Areas.Finishing.Services.Interfaces;
using Shared.ViewModels.Areas.Finishing;
using evolUX.UI.Areas.Finishing.Repositories.Interfaces;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class ExpeditionService : IExpeditionService
    {
        private readonly IExpeditionRepository _expeditionRepository;
        public ExpeditionService(IExpeditionRepository expeditionRepository)
        {
            _expeditionRepository = expeditionRepository;
        }
        public async Task<BusinessViewModel> GetCompanyBusiness(string CompanyBusinessList)
        {
            var response = await _expeditionRepository.GetCompanyBusiness(CompanyBusinessList);
            return response;
        }
        public async Task<ExpeditionFilesViewModel> GetPendingExpeditionFiles(int businessID, string ServiceCompanyList)
        {
            var response = await _expeditionRepository.GetPendingExpeditionFiles(businessID, ServiceCompanyList);
            return response;
        }
    }
}
