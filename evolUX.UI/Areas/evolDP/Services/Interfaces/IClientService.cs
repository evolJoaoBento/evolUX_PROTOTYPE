using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Services.Interfaces
{
    public interface IClientService
    {
        public Task<BusinessViewModel> GetCompanyBusiness(string CompanyBusinessList);
        public Task<ProjectListViewModel> GetProjects(string CompanyBusinessList);
    }
}
