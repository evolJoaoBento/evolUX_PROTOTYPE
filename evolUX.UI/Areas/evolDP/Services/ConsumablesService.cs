using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using Flurl.Http;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Services
{
    public class ConsumablesService : IConsumablesService
    {
        private readonly IConsumablesRepository _ConsumablesTypeRepository;
        public ConsumablesService(IConsumablesRepository ConsumablesTypeRepository)
        {
            _ConsumablesTypeRepository = ConsumablesTypeRepository;
        }
        //public async Task<ConsumablesTypeViewModel> GetConsumablesTypes(int? ConsumablesType, string expCompanyList)
        //{
        //    var response = await _ConsumablesTypeRepository.GetConsumablesTypes(ConsumablesType, expCompanyList);
        //    return response;
        //}
        //public async Task<ConsumablesZoneViewModel> GetConsumablesZones(int? ConsumablesZone, string expCompanyList)
        //{
        //    var response = await _ConsumablesTypeRepository.GetConsumablesZones(ConsumablesZone, expCompanyList);
        //    return response;
        //}

    }
}
