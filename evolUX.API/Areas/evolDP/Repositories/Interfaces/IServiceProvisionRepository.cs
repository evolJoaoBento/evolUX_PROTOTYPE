using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IServiceProvisionRepository
    {
        public Task<IEnumerable<ServiceTask>> GetServiceTasks(int? serviceTaskID);

    }
}
