﻿using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Repositories.Interfaces
{
    public interface IMaterialsRepository
    {
        public Task<IEnumerable<FulfillMaterialCode>> GetFulfillMaterialCodes();
        public Task<IEnumerable<MaterialType>> GetMaterialTypes();
        //public Task<ExpeditionTypeViewModel> GetExpeditionTypes(int? expeditionType, string expCompanyList);
        //public Task<ExpeditionZoneViewModel> GetExpeditionZones(int? expeditionZone, string expCompanyList);
    }
}