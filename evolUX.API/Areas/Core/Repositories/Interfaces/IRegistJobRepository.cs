using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using System.Data;

namespace evolUX.API.Areas.Core.Repositories.Interfaces
{
    public interface IRegistJobRepository
    {
        public Task<IEnumerable<ResourceInfo>> GetResources(string resourceType, IEnumerable<int> profileList, string valueFilter, bool ignoreProfiles);
        public Task<FlowInfo> GetFlowByCriteria(Dictionary<string, object> dictionary);
        public Task<IEnumerable<FlowParameter>> GetFlowData(int flowID);
        public Task<Result> TryRegistJob(IEnumerable<FlowParameter> flowparameters, FlowInfo flowinfo, int userID);
        public Task<IEnumerable<Job>> GetJobs(int flowID);
    }
}
