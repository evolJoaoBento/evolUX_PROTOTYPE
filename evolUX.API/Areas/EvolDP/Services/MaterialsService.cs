using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using Shared.Models.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Services
{
    public class MaterialsService : IMaterialsService
    {
        private readonly IWrapperRepository _repository;

        public MaterialsService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes(string fullFillMaterialCode)
        {
            IEnumerable<FulfillMaterialCode> result = await _repository.Materials.GetFulfillMaterialCodes(fullFillMaterialCode);
            return result;
        }
        public async Task<IEnumerable<MaterialType>> GetMaterialTypes()
        {
            IEnumerable<MaterialType> result = await _repository.Materials.GetMaterialTypes();
            return result;
        }
        public async Task<MaterialElement> SetMaterialGroup(MaterialElement group, DataTable serviceCompanyList)
        {
            int groupID = await _repository.Materials.SetMaterialGroup(group, serviceCompanyList);
            IEnumerable<MaterialElement> result = await _repository.Materials.GetMaterialGroups(groupID, "", 0, "", serviceCompanyList);
            return result?.First();
        }
        public async Task<IEnumerable<MaterialElement>> GetMaterials(int materialID, string materialRef, string materialCode, int groupID, int materialTypeID, string materialTypeCode, DataTable serviceCompanyList)
        {
            IEnumerable<MaterialElement> result = await _repository.Materials.GetMaterials(materialID, materialRef, materialCode, groupID, materialTypeID, materialTypeCode, serviceCompanyList);
            return result;
        }
        public async Task<MaterialElement> SetMaterial(MaterialElement material, string materialTypeCode, DataTable serviceCompanyList)
        {
            int materialID = await _repository.Materials.SetMaterial(material, serviceCompanyList);
            if (material.GroupID > 0)
            {
                IEnumerable<MaterialElement> result = await _repository.Materials.GetMaterialGroups(material.GroupID, "", string.IsNullOrEmpty(materialTypeCode) ? material.MaterialTypeID : 0, materialTypeCode, serviceCompanyList);
                return result?.First();
            }
            else
                return new MaterialElement();
        }
        public async Task<IEnumerable<MaterialCostElement>> GetMaterialCost(int materialID, DataTable serviceCompanyList)
        {
            var response = await _repository.Materials.GetMaterialCost(materialID, serviceCompanyList);
            return response;
        }
        public async Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMedia(int? envMediaID)
        {
            var envelopeMediaList = await _repository.Materials.GetEnvelopeMedia(envMediaID);
            if (envelopeMediaList == null)
            {

            }
            return envelopeMediaList;
        }        
        public async Task<IEnumerable<EnvelopeMediaGroup>> GetEnvelopeMediaGroups(int? envMediaGroupID)
        {
            var envelopeMediaGroupList = await _repository.Materials.GetEnvelopeMediaGroups(envMediaGroupID);
            if (envelopeMediaGroupList == null)
            {

            }
            return envelopeMediaGroupList;
        }
    }
}
