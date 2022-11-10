using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.API.Areas.Finishing.ViewModels;
using evolUX.UI.Repositories;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services
{
    public class PrintService : IPrintService
    {
        private readonly IPrintRepository _printRepository;
        public PrintService(IPrintRepository printRepository)
        {
            _printRepository = printRepository;
        }
        public async Task<ResoursesViewModel> GetPrinters(string profileList, string filesSpecs, bool ignoreProfiles)
        {
            var response = await _printRepository.GetPrinters(profileList, filesSpecs, ignoreProfiles);
            return response;
        }
    }
}
