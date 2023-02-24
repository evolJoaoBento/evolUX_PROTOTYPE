using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Repositories.Interfaces
{
    public interface IClientRepository
    {
        public Task<BusinessViewModel> GetCompanyBusiness(string CompanyBusinessList);
        public Task<ProjectListViewModel> GetProjects(string CompanyBusinessList);
    

    }
}