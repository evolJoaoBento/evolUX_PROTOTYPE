using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IGenericRepository
    {
        public Task<IEnumerable<Company>> GetCompanies(int? companyID, DataTable? CompanyList);
        public Task<IEnumerable<Business>> GetCompanyBusiness(DataTable CompanyBusinessList);
        public Task<IEnumerable<ProjectElement>> GetProjects(DataTable CompanyBusinessList);
        public Task<IEnumerable<ConstantParameter>> GetParameters();
        public Task<IEnumerable<ConstantParameter>> SetParameter(int parameterID, string parameterRef, int parameterValue, string parameterDescription);
        public Task<IEnumerable<ConstantParameter>> DeleteParameter(int parameterID);
    }
}
