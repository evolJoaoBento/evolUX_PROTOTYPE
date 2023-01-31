using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.EvolDP.Services.Interfaces;

namespace evolUX.API.Areas.EvolDP.Services
{
    public class ExpeditionTypeService : IExpeditionTypeService
    {
        private readonly IWrapperRepository _repository;
        public ExpeditionTypeService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<dynamic>> GetExpeditionTypes()
        {
            var expeditionTypeList = await _repository.ExpeditionType.GetExpeditionTypes();
            if (expeditionTypeList == null)
            {

            }
            return expeditionTypeList;
        }
    }
}
