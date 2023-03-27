using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Repositories.Interfaces
{
    public interface IConsumablesRepository
    {
        public Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes();
        //public Task<ExpeditionTypeViewModel> GetExpeditionTypes(int? expeditionType, string expCompanyList);
        //public Task<ExpeditionZoneViewModel> GetExpeditionZones(int? expeditionZone, string expCompanyList);
    }
}
