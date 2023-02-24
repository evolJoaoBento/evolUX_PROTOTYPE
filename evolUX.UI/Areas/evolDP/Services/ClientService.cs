using Shared.ViewModels.Areas.evolDP;
using evolUX.UI.Exceptions;
using Flurl.Http;
using Shared.Models.Areas.Core;
using Shared.ViewModels.Areas.Core;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using evolUX.UI.Areas.evolDP.Repositories.Interfaces;

namespace evolUX.UI.Areas.evolDP.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clienttRepository;
        public ClientService(IClientRepository clientRepository)
        {
            _clienttRepository = clientRepository;
        }
        public async Task<BusinessViewModel> GetCompanyBusiness(string CompanyBusinessList)
        {
            var response = await _clienttRepository.GetCompanyBusiness(CompanyBusinessList);
            return response;
        }
        public async Task<ProjectListViewModel> GetProjects(string CompanyBusinessList)
        {
            var response = await _clienttRepository.GetProjects(CompanyBusinessList);
            return response;
        }
        public async Task<ConstantParameterViewModel> GetParameters()
        {
            var response = await _clienttRepository.GetParameters();
            return response;
        }
        public async Task<ConstantParameterViewModel> SetParameter(int parameterID, string parameterRef, int parameterValue, string parameterDescription)
        {
            var response = await _clienttRepository.SetParameter(parameterID, parameterRef, parameterValue, parameterDescription);
            return response;
        }
        public async Task<ConstantParameterViewModel> DeleteParameter(int parameterID)
        {
            var response = await _clienttRepository.DeleteParameter(parameterID);
            return response;
        }

    }
}

