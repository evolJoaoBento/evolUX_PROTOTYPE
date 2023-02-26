using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using Flurl.Http;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Services
{
    public class ServiceProvisionService : IServiceProvisionService
    {
        private readonly IServiceProvisionRepository _ServiceProvisionTypeRepository;
        public ServiceProvisionService(IServiceProvisionRepository ServiceProvisionTypeRepository)
        {
            _ServiceProvisionTypeRepository = ServiceProvisionTypeRepository;
        }
        //public async Task<ServiceProvisionTypeViewModel> GetServiceProvisionTypes(int? ServiceProvisionType, string expCompanyList)
        //{
        //    var response = await _ServiceProvisionTypeRepository.GetServiceProvisionTypes(ServiceProvisionType, expCompanyList);
        //    return response;
        //}
        //public async Task<ServiceProvisionZoneViewModel> GetServiceProvisionZones(int? ServiceProvisionZone, string expCompanyList)
        //{
        //    var response = await _ServiceProvisionTypeRepository.GetServiceProvisionZones(ServiceProvisionZone, expCompanyList);
        //    return response;
        //}

    }
}
