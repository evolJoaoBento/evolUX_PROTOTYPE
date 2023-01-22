using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Repositories.Interfaces;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class ConcludedFullfillService : IConcludedFullfillService
    {
        private readonly IConcludedFullfillRepository _concludedFullfillRepository;
        public ConcludedFullfillService(IConcludedFullfillRepository concludedFullfillRepository)
        {
            _concludedFullfillRepository = concludedFullfillRepository;
        }
        public async Task<IFlurlResponse> RegistFullFill(string FileBarcode, string user, string ServiceCompanyList)
        {
            var response = await _concludedFullfillRepository.RegistFullFill(FileBarcode, user, ServiceCompanyList);
            return response;
        }
    }
}
