﻿using Shared.Models.Areas.evolDP;

namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IMaterialsRepository
    {
        public Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes(string fullFillMaterialCode);
        public Task<IEnumerable<MaterialType>> GetMaterialTypes(bool groupCodes, string materialTypeCode);
        public Task<IEnumerable<MaterialElement>> GetMaterialGroups(int groupID, string groupCode, int materialTypeID, string materialTypeCode);
        public Task SetMaterialGroup(MaterialElement group);
        public Task<IEnumerable<MaterialElement>> GetMaterials(int materialID, string materialRef, string materialCode, int groupID, int materialTypeID, string materialTypeCode);
        public Task SetMaterial(MaterialElement material);
        public Task<IEnumerable<EnvelopeMediaGroup>> GetEnvelopeMediaGroups(int? envMediaGroupID);
        public Task<IEnumerable<EnvelopeMedia>> GetEnvelopeMedia(int? envMediaID);
    }
}
