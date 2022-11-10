using SharedModels.ViewModels.Areas.Finishing;
using SharedModels.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IPrintService
    {
        public Task<IEnumerable<ResourceInfo>> GetPrinters(IEnumerable<int> profileList, string filesSpecs, bool ignoreProfiles);
        
    }
}
