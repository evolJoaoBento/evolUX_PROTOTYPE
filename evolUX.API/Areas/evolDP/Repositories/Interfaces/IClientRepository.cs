using Shared.Models.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Repositories.Interfaces
{
    public interface IClientRepository
    {
        public Task<IEnumerable<ProjectElement>> GetProjects(DataTable CompanyBusinessList); 
    }
}
