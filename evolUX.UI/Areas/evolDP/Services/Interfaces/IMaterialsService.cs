using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Services.Interfaces
{
    public interface IMaterialsService
    {
        public Task<IEnumerable<FullfillMaterialCode>> GetFulfillMaterialCodes();
        public Task<IEnumerable<MaterialType>> GetMaterialTypes(string materialTypeCode);
        public Task<IEnumerable<MaterialType>> GetMaterialTypes(bool groupCodes, string materialTypeCode);
        public Task<IEnumerable<MaterialElement>> GetMaterialGroups(string materialTypeCode, string serviceCompanyList);
        public Task<MaterialElement> SetMaterialGroup(MaterialElement group, string serviceCompanyList);
        public Task<IEnumerable<MaterialElement>> GetMaterials(int groupID, string materialTypeCode, string serviceCompanyList);
        public Task<MaterialElement> SetMaterial(MaterialElement material, string materialTypeCode, string serviceCompanyList);
        //public Task<MaterialsTypeViewModel> GetMaterialsTypes(int? MaterialsType, string expCompanyList);
        //public Task<MaterialsZoneViewModel> GetMaterialsZones(int? MaterialsZone, string expCompanyList);
    }
}
