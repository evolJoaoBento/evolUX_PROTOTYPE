using evolUX.API.Models;
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
        public async Task<IEnumerable<FullfillMaterialCode>> GetFulfillMaterialCodes()
        {
            var response = await _materialsRepository.GetFulfillMaterialCodes();
            return response;
        }
        public async Task<IEnumerable<MaterialType>> GetMaterialTypes(bool groupCodes, string materialTypeCode)
        {
            var response = await _materialsRepository.GetMaterialTypes(groupCodes, materialTypeCode);
            return response;
        }
        public async Task<IEnumerable<MaterialType>> GetMaterialTypes(string materialTypeCode)
        {
            var response = await _materialsRepository.GetMaterialTypes(false, materialTypeCode);
            return response;
        }
        public async Task<IEnumerable<MaterialElement>> GetMaterialGroups(string materialTypeCode, string serviceCompanyList)
        {
            var response = await _materialsRepository.GetMaterialGroups(materialTypeCode, serviceCompanyList);
            return response;
        }
        public async Task<MaterialElement> SetMaterialGroup(MaterialElement group, string serviceCompanyList)
        {
            var response = await _materialsRepository.SetMaterialGroup(group, serviceCompanyList);
            return response;

        }
        public async Task<IEnumerable<MaterialElement>> GetMaterials(int groupID, string materialTypeCode, string serviceCompanyList)
        {
            var response = await _materialsRepository.GetMaterials(groupID, materialTypeCode, serviceCompanyList);
            return response;
        }
        public async Task<MaterialElement> SetMaterial(MaterialElement material, string materialTypeCode, string serviceCompanyList)
        {
            var response = await _materialsRepository.SetMaterial(material, materialTypeCode, serviceCompanyList);
            return response;
        }
        public async Task<IEnumerable<MaterialCostElement>> GetMaterialCost(int materialID, string serviceCompanyList)
        {
            var response = await _materialsRepository.GetMaterialCost(materialID, serviceCompanyList);
            return response;
        }
        public async Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int materialTypeID)
        {
            var response = await _materialsRepository.GetServiceCompanyRestrictions(materialTypeID);
            return response;
        }
    }
}
