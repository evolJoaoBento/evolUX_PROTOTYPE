using evolUX.API.Areas.Finishing.ViewModels;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IPrintService
    {
        public Task<ResoursesViewModel> GetPrinters(string profileList, string filesSpecs, bool ignoreProfiles);
    }
}
