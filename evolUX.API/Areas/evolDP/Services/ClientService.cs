using System.Data;
using Shared.Models.Areas.evolDP;
using Shared.Models.Areas.Finishing;
using Shared.Models.General;
using Shared.Models.Areas.Core;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.API.Areas.evolDP.Services
{
    public class ClientService : IClientService
    {
        private readonly IWrapperRepository _repository;
        public ClientService(IWrapperRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<ProjectListViewModel> GetProjects(DataTable CompanyBusinessList)
        {
            ProjectListViewModel viewmodel = new ProjectListViewModel();

            viewmodel.Projects = (List<ProjectElement>)await _repository.Project.GetProjects(CompanyBusinessList);
            return viewmodel;
        }
    }
}
