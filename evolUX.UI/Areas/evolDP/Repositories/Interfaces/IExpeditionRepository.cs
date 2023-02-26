﻿using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Repositories.Interfaces
{
    public interface IExpeditionRepository
    {
        public Task<ExpeditionTypeViewModel> GetExpeditionTypes(int? expeditionType, string expCompanyList);
        public Task<ExpeditionTypeViewModel> GetExpCompanyTypes(int? expeditionType, int? expCompanyID);
        public Task<ExpeditionTypeViewModel> SetExpCompanyType(int expeditionType, int expCompanyID, bool registMode, bool separationMode, bool barcodeRegistMode, bool returnAll);
        public Task<ExpeditionZoneViewModel> GetExpeditionZones(int? expeditionZone, string expCompanyList);
    }
}
