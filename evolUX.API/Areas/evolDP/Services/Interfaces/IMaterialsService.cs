using Shared.Models.Areas.evolDP;

namespace evolUX.API.Areas.evolDP.Services.Interfaces
{
    public interface IMaterialsService
    {
        public Task<IEnumerable<FullfillMaterialCode>> GetFulfillMaterialCodes(string fullFillMaterialCode);
        public Task<IEnumerable<MaterialType>> GetMaterialTypes(bool groupCodes, string materialTypeCode);
        public Task<IEnumerable<MaterialElement>> GetMaterialGroups(int groupID, string groupCode, int materialTypeID, string materialTypeCode, DataTable serviceCompanyList);
        public Task<MaterialElement> SetMaterialGroup(MaterialElement group, DataTable serviceCompanyList);
        public Task<IEnumerable<MaterialElement>> GetMaterials(int materialID, string materialRef, string materialCode, int groupID, int materialTypeID, string materialTypeCode, DataTable serviceCompanyList);
        public Task<MaterialElement> SetMaterial(MaterialElement material, string materialTypeCode, DataTable serviceCompanyList);
        public Task<IEnumerable<MaterialCostElement>> GetMaterialCost(int materialID, DataTable serviceCompanyList);
        public Task<IEnumerable<EnvelopeMediaGroup>> GetEnvelopeMediaGroups(int? envMediaGroupID);
        public Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMedia(int? envMediaID);
    }
}
