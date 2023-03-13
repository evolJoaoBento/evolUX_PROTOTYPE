using Flurl.Http;
using System.Data;
using Shared.ViewModels.General;
using Shared.Models.General;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;

namespace evolUX.UI.Areas.Finishing.Services.Interfaces
{
    public interface IPrintService
    {
        public Task<PrinterViewModel> GetPrinters(string profileList, string filesSpecs, bool ignoreProfiles);
        public Task<Result> Print(string printer, string serviceCompanyCode, string username, int userID, List<PrintFileInfo> prodFiles);
    }
}
