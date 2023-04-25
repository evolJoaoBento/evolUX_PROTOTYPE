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
        public async Task<IEnumerable<MaterialType>> GetMaterialTypes(bool groupCodes, string materialTypeCode)
        {
            var response = await _materialsRepository.GetMaterialTypes(groupCodes, materialTypeCode);
            return response;
        }
        public async Task<IEnumerable<MaterialElement>> GetMaterialGroups(string materialTypeCode)
        {
            var response = await _materialsRepository.GetMaterialGroups(materialTypeCode);
            return response;
        }
        public async Task SetMaterialGroup(MaterialElement group)
        {
            await _materialsRepository.SetMaterialGroup(group);
            return;

        }
        public async Task<IEnumerable<MaterialElement>> GetMaterials(int groupID, string materialTypeCode)
        {
            var response = await _materialsRepository.GetMaterials(groupID, materialTypeCode);
            return response;
        }
        public async Task SetMaterial(MaterialElement material)
        {
            await _materialsRepository.SetMaterialGroup(material);
            return;
        }
    }
}
