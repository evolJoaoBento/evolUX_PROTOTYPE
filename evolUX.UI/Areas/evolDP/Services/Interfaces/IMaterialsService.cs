using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Services.Interfaces
{
    public interface IMaterialsService
    {
        public Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes();
        public Task<IEnumerable<MaterialType>> GetMaterialTypes(bool groupCodes, string materialTypeCode);
        public Task<IEnumerable<MaterialElement>> GetMaterialGroups(string materialTypeCode);
        public Task<IEnumerable<MaterialElement>> GetMaterials(int groupID, string materialTypeCode);
        //public Task<MaterialsTypeViewModel> GetMaterialsTypes(int? MaterialsType, string expCompanyList);
        //public Task<MaterialsZoneViewModel> GetMaterialsZones(int? MaterialsZone, string expCompanyList);
    }
}
