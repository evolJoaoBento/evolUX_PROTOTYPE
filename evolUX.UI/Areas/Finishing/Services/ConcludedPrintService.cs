using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Repositories.Interfaces;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class ConcludedPrintService : IConcludedPrintService
    {
        private readonly IConcludedPrintRepository _concludedPrintRepository;
        public ConcludedPrintService(IConcludedPrintRepository concludedPrintRepository)
        {
            _concludedPrintRepository = concludedPrintRepository;
        }
        public async Task<IFlurlResponse> RegistPrint(string FileBarcode, string user, DataTable ServiceCompanyList)
        {
            var response = await _concludedPrintRepository.RegistPrint(FileBarcode, user, ServiceCompanyList);
            return response;
        }
    }
}
