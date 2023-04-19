using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Services.Interfaces
{
    public interface IGenericService
    {
        public Task<BusinessViewModel> GetCompanyBusiness(string companyBusinessList);
        public Task<BusinessViewModel> GetCompanyBusiness(int companyID);
        public Task<Company> SetCompany(Company company);
        public Task SetBusiness(Business business);
        public Task<ProjectListViewModel> GetProjects(string companyBusinessList);
        public Task<ConstantParameterViewModel> GetParameters();
        public Task<ConstantParameterViewModel> SetParameter(int parameterID, string parameterRef, int parameterValue, string parameterDescription);
        public Task<ConstantParameterViewModel> DeleteParameter(int parameterID);
    }
}
