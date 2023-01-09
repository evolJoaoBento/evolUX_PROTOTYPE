

using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IPrintFilesRepository
    {
        public Task<IEnumerable<ResourceInfo>> GetPrinters(IEnumerable<int> profileList, string filesSpecs, bool ignoreProfiles);
        public Task<FlowInfo> GetFlow(string ServiceCompanyCode);
        public Task<IEnumerable<FlowParameter>> GetFlowParameters(int flowID);
        public Task<Result> TryPrint(IEnumerable<FlowParameter> flowparameters, FlowInfo flowinfo, int userID);
        public Task LogSentToPrinter(int runID, int fileID);
    }
}
