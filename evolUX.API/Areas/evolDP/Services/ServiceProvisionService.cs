using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using Shared.Models.Areas.evolDP;

namespace evolUX.API.Areas.evolDP.Services
{
    public class ServiceProvisionService : IServiceProvisionService
    {
        private readonly IWrapperRepository _repository;

        public ServiceProvisionService(IWrapperRepository repository)
        {
            _repository = repository;
        }

    }
}
