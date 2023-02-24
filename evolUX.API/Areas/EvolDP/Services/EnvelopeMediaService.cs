using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;

namespace evolUX.API.Areas.evolDP.Services
{
    public class EnvelopeMediaService : IEnvelopeMediaService
    {
        private readonly IWrapperRepository _repository;

        public EnvelopeMediaService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<dynamic>> GetEnvelopeMedia()
        {
            var envelopeMediaList = await _repository.EnvelopeMedia.GetEnvelopeMedia();
            if (envelopeMediaList == null)
            {

            }
            return envelopeMediaList;
        }        
        public async Task<List<dynamic>> GetEnvelopeMediaGroups()
        {
            var envelopeMediaGroupList = await _repository.EnvelopeMedia.GetEnvelopeMediaGroups();
            if (envelopeMediaGroupList == null)
            {

            }
            return envelopeMediaGroupList;
        }
    }
}
