using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Services.Interfaces
{
    public interface IConsumablesService
    {
        public Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes();
        //public Task<ConsumablesTypeViewModel> GetConsumablesTypes(int? ConsumablesType, string expCompanyList);
        //public Task<ConsumablesZoneViewModel> GetConsumablesZones(int? ConsumablesZone, string expCompanyList);
    }
}
