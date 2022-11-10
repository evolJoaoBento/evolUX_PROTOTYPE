using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Repositories;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class ConcludedEnvelopeService : IConcludedEnvelopeService
    {
        private readonly IConcludedEnvelopeRepository _concludedEnvelopeRepository;
        public ConcludedEnvelopeService(IConcludedEnvelopeRepository concludedEnvelopeRepository)
        {
            _concludedEnvelopeRepository = concludedEnvelopeRepository;
        }
        public async Task<IFlurlResponse> RegistFullFill(string FileBarcode, string user, DataTable ServiceCompanyList)
        {
            var response = await _concludedEnvelopeRepository.RegistFullFill(FileBarcode, user, ServiceCompanyList);
            return response;
        }
    }
}
