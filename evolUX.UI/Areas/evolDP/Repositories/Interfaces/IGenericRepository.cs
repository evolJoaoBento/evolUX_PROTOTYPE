using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Repositories.Interfaces
{
    public interface IGenericRepository
    {
        public Task<BusinessViewModel> GetCompanyBusiness(string CompanyBusinessList);
        public Task<ProjectListViewModel> GetProjects(string CompanyBusinessList);
        public Task<ConstantParameterViewModel> GetParameters();
        public Task<ConstantParameterViewModel> SetParameter(int parameterID, string parameterRef, int parameterValue, string parameterDescription);
        public Task<ConstantParameterViewModel> DeleteParameter(int parameterID);
    }
}