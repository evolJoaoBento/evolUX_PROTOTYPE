using System.Data;
using Shared.Models.Areas.evolDP;
using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using Shared.Models.Areas.Core;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.API.Areas.evolDP.Services
{
    public class GenericService : IGenericService
    {
        private readonly IWrapperRepository _repository;
        public GenericService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Company>> GetCompanies(DataTable CompanyList)
        {
            IEnumerable<Company> result = await _repository.Generic.GetCompanies(null, CompanyList);
            return result;
        }
        public async Task<IEnumerable<Company>> GetCompanies(int CompanyID)
        {
            IEnumerable<Company> result = await _repository.Generic.GetCompanies(CompanyID, null);
            return result;
        }
        public async Task<Company> SetCompany(Company company)
        {
            int companyID = await _repository.Generic.SetCompany(company);
            IEnumerable<Company> list = await _repository.Generic.GetCompanies(companyID, null);
            if (list == null)
            {

            }

            return list.First();
        }

        public async Task<IEnumerable<Business>> GetCompanyBusiness(int companyID, DataTable CompanyList)
        {
            IEnumerable<Business> companyBusiness = await _repository.Generic.GetCompanyBusiness(companyID, CompanyList);
            if (companyBusiness == null)
            {

            }

            return companyBusiness;
        }
        public async Task SetBusiness(Business business)
        {
            await _repository.Generic.SetBusiness(business);
            return;
        }
        public async Task<ProjectListViewModel> GetProjects(DataTable CompanyBusinessList)
        {
            ProjectListViewModel viewmodel = new ProjectListViewModel();

            viewmodel.Projects = (List<ProjectElement>)await _repository.Generic.GetProjects(CompanyBusinessList);
            return viewmodel;
        }
        public async Task<ConstantParameterViewModel> GetParameters()
        {
            ConstantParameterViewModel viewmodel = new ConstantParameterViewModel();
            viewmodel.ConstantsList = await _repository.Generic.GetParameters();
            return viewmodel;
        }

        public async Task<ConstantParameterViewModel> SetParameter(int parameterID, string parameterRef, int parameterValue, string parameterDescription)
        {
            ConstantParameterViewModel viewmodel = new ConstantParameterViewModel();
            viewmodel.ConstantsList = await _repository.Generic.SetParameter(parameterID, parameterRef, parameterValue, parameterDescription);
            return viewmodel;
        }

        public async Task<ConstantParameterViewModel> DeleteParameter(int parameterID)
        {
            ConstantParameterViewModel viewmodel = new ConstantParameterViewModel();
            viewmodel.ConstantsList = await _repository.Generic.DeleteParameter(parameterID);
            return viewmodel;
        }
    }
}
