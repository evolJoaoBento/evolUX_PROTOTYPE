using System.Data;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.API.Areas.evolDP.Services.Interfaces
{
    public interface IGenericService
    {
        public Task<IEnumerable<Company>> GetCompanies(DataTable CompanyList);
        public Task<IEnumerable<Company>> GetCompanies(int CompanyID);
        public Task<Company> SetCompany(Company company);
        public Task<IEnumerable<Business>> GetCompanyBusiness(int companyID, DataTable CompanyList);
        public Task SetBusiness(Business business);
        public Task<ProjectListViewModel> GetProjects(DataTable CompanyBusinessList);
        public Task<ConstantParameterViewModel> GetParameters();
        public Task<ConstantParameterViewModel> SetParameter(int parameterID, string parameterRef, int parameterValue, string parameterDescription);
        public Task<ConstantParameterViewModel> DeleteParameter(int parameterID);
    }
}
