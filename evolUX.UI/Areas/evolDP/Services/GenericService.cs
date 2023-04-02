using Shared.ViewModels.Areas.evolDP;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using evolUX.UI.Areas.evolDP.Repositories.Interfaces;

namespace evolUX.UI.Areas.evolDP.Services
{
    public class GenericService : IGenericService
    {
        private readonly IGenericRepository _genericRepository;
        public GenericService(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<BusinessViewModel> GetCompanyBusiness(string CompanyBusinessList)
        {
            var response = await _genericRepository.GetCompanyBusiness(CompanyBusinessList);
            return response;
        }
        public async Task<BusinessViewModel> GetCompanyBusiness(int CompanyID)
        {
            var response = await _genericRepository.GetCompanyBusiness(CompanyID);
            return response;
        }
        public async Task<ProjectListViewModel> GetProjects(string CompanyBusinessList)
        {
            var response = await _genericRepository.GetProjects(CompanyBusinessList);
            return response;
        }
        public async Task<ConstantParameterViewModel> GetParameters()
        {
            var response = await _genericRepository.GetParameters();
            return response;
        }
        public async Task<ConstantParameterViewModel> SetParameter(int parameterID, string parameterRef, int parameterValue, string parameterDescription)
        {
            var response = await _genericRepository.SetParameter(parameterID, parameterRef, parameterValue, parameterDescription);
            return response;
        }
        public async Task<ConstantParameterViewModel> DeleteParameter(int parameterID)
        {
            var response = await _genericRepository.DeleteParameter(parameterID);
            return response;
        }

    }
}

