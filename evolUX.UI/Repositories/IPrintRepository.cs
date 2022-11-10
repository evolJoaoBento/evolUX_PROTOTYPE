using evolUX.API.Areas.Finishing.Models;
using evolUX.API.Areas.Finishing.ViewModels;
using Flurl.Http;
using System.Data;

namespace evolUX.UI.Repositories
{
    public interface IPrintRepository
    {
        public Task<ResoursesViewModel> GetPrinters(string profileList, string filesSpecs, bool ignoreProfiles);
    }
}