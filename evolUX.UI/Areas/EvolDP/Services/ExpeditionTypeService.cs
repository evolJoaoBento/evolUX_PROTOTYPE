﻿using evolUX.UI.Areas.EvolDP.Services.Interfaces;
using evolUX.UI.Repositories;
using Flurl.Http;

namespace evolUX.UI.Areas.EvolDP.Services
{
    public class ExpeditionTypeService : IExpeditionTypeService
    {
        private readonly IExpeditionTypeRepository _expeditionTypeRepository;

        public ExpeditionTypeService(IExpeditionTypeRepository expeditionTypeRepository)
        {
            _expeditionTypeRepository = expeditionTypeRepository;
        }
        public async Task<IFlurlResponse> GetExpeditionTypes()
        {
            var response = await _expeditionTypeRepository.GetExpeditionTypes();
            return response;
        }
    }
}
