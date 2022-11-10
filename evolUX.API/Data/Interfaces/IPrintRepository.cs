

using SharedModels.Models.Areas.Finishing;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IPrintRepository
    {
        public Task<IEnumerable<ResourceInfo>> GetPrinters(IEnumerable<int> profileList, string filesSpecs, bool ignoreProfiles);
        public Task<FlowInfo> GetFlowID(string ServiceCompanyCode);
    }
}
