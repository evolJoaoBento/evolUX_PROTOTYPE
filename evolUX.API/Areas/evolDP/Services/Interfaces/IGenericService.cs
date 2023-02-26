using System.Data;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.API.Areas.evolDP.Services.Interfaces
{
    public interface IGenericService
    {
        public Task<IEnumerable<Business>> GetCompanyBusiness(DataTable CompanyBusinessList);
        public Task<ProjectListViewModel> GetProjects(DataTable CompanyBusinessList);
        public Task<ConstantParameterViewModel> GetParameters();
        public Task<ConstantParameterViewModel> SetParameter(int parameterID, string parameterRef, int parameterValue, string parameterDescription);
        public Task<ConstantParameterViewModel> DeleteParameter(int parameterID);
    }
}
