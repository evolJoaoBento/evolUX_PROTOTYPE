﻿using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using evolUX.API.Models;
using Shared.Models.Areas.evolDP;
using System.Data;
using System.Text.RegularExpressions;

namespace evolUX.API.Areas.evolDP.Services
{
    public class MaterialsService : IMaterialsService
    {
        private readonly IWrapperRepository _repository;

        public MaterialsService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<FullfillMaterialCode>> GetFulfillMaterialCodes(string fullFillMaterialCode)
        {
            IEnumerable<FullfillMaterialCode> result = await _repository.Materials.GetFulfillMaterialCodes(fullFillMaterialCode);
            return result;
        }
        public async Task<IEnumerable<MaterialType>> GetMaterialTypes(bool groupCodes, string materialTypeCode)
        {
            IEnumerable<MaterialType> result = await _repository.Materials.GetMaterialTypes(groupCodes, materialTypeCode);
            return result;
        }
        public async Task<IEnumerable<MaterialElement>> GetMaterialGroups(int groupID, string groupCode, int materialTypeID, string materialTypeCode, DataTable serviceCompanyList)
        {
            IEnumerable<MaterialElement> result = await _repository.Materials.GetMaterialGroups(groupID, groupCode, materialTypeID, materialTypeCode, serviceCompanyList);
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
        public async Task<MaterialElement> SetMaterial(MaterialElement material, DataTable serviceCompanyList)
        {
            int materialID = await _repository.Materials.SetMaterial(material, serviceCompanyList);
            IEnumerable<MaterialElement> result = await _repository.Materials.GetMaterials(materialID, "", "", 0, 0, "", serviceCompanyList);
            return result?.First();
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
