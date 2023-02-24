using System.Data;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.API.Areas.evolDP.Services.Interfaces
{
    public interface IClientService
    {
        public Task<ProjectListViewModel> GetProjects(DataTable CompanyBusinessList);
    }
}
