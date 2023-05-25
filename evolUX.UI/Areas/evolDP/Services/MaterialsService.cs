using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Services
{
    public class MaterialsService : IMaterialsService
    {
        private readonly IMaterialsRepository _materialsRepository;
        public MaterialsService(IMaterialsRepository materialsRepository)
        {
            _materialsRepository = materialsRepository;
        }
        public async Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes()
        {
            var response = await _materialsRepository.GetFulfillMaterialCodes();
            return response;
        }
        public async Task<IEnumerable<MaterialType>> GetMaterialTypes()
        {
            var response = await _materialsRepository.GetMaterialTypes();
            return response;
        }
        //public async Task<MaterialsTypeViewModel> GetMaterialsTypes(int? MaterialsType, string expCompanyList)
        //{
        //    var response = await _MaterialsTypeRepository.GetMaterialsTypes(MaterialsType, expCompanyList);
        //    return response;
        //}
        //public async Task<MaterialsZoneViewModel> GetMaterialsZones(int? MaterialsZone, string expCompanyList)
        //{
        //    var response = await _MaterialsTypeRepository.GetMaterialsZones(MaterialsZone, expCompanyList);
        //    return response;
        //}

    }
}
