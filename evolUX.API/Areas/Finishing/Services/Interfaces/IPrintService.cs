using Shared.ViewModels.Areas.Finishing;
using Shared.Models.Areas.Finishing;
using System.Data;
using Shared.Models.General;
using Shared.ViewModels.General;

namespace evolUX.API.Areas.Finishing.Services.Interfaces
{
    public interface IPrintService
    {
        public Task<ResoursesViewModel> GetPrinters(IEnumerable<int> profileList, string filesSpecs, bool ignoreProfiles);
        public Task<ResultsViewModel> Print(int runID, int fileID, string printer, string serviceCompanyCode, string username, int userID, string filePath, string fileName, string shortFileName);
    }
}
