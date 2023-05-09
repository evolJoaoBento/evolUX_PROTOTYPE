using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Repositories.Interfaces
{
    public interface IMaterialsRepository
    {
        public Task<IEnumerable<FullfillMaterialCode>> GetFulfillMaterialCodes();
        public Task<IEnumerable<MaterialType>> GetMaterialTypes(bool groupCodes, string materialTypeCode);
        public Task<IEnumerable<MaterialElement>> GetMaterialGroups(string materialTypeCode, string serviceCompanyList);
        public Task<MaterialElement> SetMaterialGroup(MaterialElement group, string serviceCompanyList);
        public Task<IEnumerable<MaterialElement>> GetMaterials(int groupID, string materialTypeCode, string serviceCompanyList);
        public Task<MaterialElement> SetMaterial(MaterialElement material, string serviceCompanyList);
        //public Task<ExpeditionTypeViewModel> GetExpeditionTypes(int? expeditionType, string expCompanyList);
        //public Task<ExpeditionZoneViewModel> GetExpeditionZones(int? expeditionZone, string expCompanyList);
    }
}
