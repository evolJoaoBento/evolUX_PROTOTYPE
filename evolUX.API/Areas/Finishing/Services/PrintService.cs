using SharedModels.Models.Areas.Finishing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using SharedModels.ViewModels.Areas.Finishing;
using evolUX.API.Data.Interfaces;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services
{
    public class PrintService : IPrintService
    {
        private readonly IWrapperRepository _repository;
        public PrintService(IWrapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ResourceInfo>> GetPrinters(IEnumerable<int> profileList, string filesSpecs, bool ignoreProfiles)
        {
            IEnumerable<ResourceInfo> printers = await _repository.Print.GetPrinters(profileList, filesSpecs, ignoreProfiles);
            if (printers == null)
            {

            }
            return printers;
        }

    }
}
