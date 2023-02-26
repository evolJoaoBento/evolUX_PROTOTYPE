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

        public async Task<IEnumerable<Business>> GetCompanyBusiness(DataTable CompanyBusinessList)
        {
            IEnumerable<Business> companyBusiness = await _repository.Generic.GetCompanyBusiness(CompanyBusinessList);
            if (companyBusiness == null)
            {

            }

            return companyBusiness;
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
