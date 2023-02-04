using Flurl.Http;
using System.Data;
using Shared.ViewModels.General;
using Shared.Models.General;
using Shared.ViewModels.Areas.Core;
using Shared.ViewModels.Areas.Finishing;

namespace evolUX.UI.Areas.Finishing.Repositories.Interfaces
{
    public interface IPrintRepository
    {
        public Task<PrinterViewModel> GetPrinters(string profileList, string filesSpecs, bool ignoreProfiles);
        public Task<Result> Print(int runID, int fileID, string printer, string serviceCompanyCode, string username, int userID, string filePath, string fileName, string shortFileName);

    }
}