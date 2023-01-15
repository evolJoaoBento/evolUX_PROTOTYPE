using Shared.ViewModels.Areas.Finishing;
using Flurl.Http;
using System.Data;
using Shared.ViewModels.General;
using Shared.Models.General;

namespace evolUX.UI.Repositories.Interfaces
{
    public interface IPrintRepository
    {
        public Task<ResoursesViewModel> GetPrinters(string profileList, string filesSpecs, bool ignoreProfiles);
        public Task<Result> Print(int runID, int fileID, string printer, string serviceCompanyCode, string username, int userID, string filePath, string fileName, string shortFileName);

    }
}