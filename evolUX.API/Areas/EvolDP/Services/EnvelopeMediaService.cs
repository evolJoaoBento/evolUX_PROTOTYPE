using evolUX.API.Areas.EvolDP.Services.Interfaces;
using evolUX.API.Data.Interfaces;

namespace evolUX.API.Areas.EvolDP.Services
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
