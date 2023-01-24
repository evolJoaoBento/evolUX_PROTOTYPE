

using Shared.Models.Areas.Core;
using Shared.Models.General;
using System.Data;

namespace evolUX.API.Data.Interfaces
{
    public interface IRegistJobRepository
    {
        public Task<FlowInfo> GetFlowByCriteria(Dictionary<string, object> dictionary);
        public Task<IEnumerable<FlowParameter>> GetFlowData(int flowID);
        public Task<Result> TryRegistJob(IEnumerable<FlowParameter> flowparameters, FlowInfo flowinfo, int userID);
        public Task<IEnumerable<Job>> GetJobs(int flowID);
    }
}
