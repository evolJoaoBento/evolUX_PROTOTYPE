using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ProjectListViewModel: ItemPermissions
    {
        public IEnumerable<ProjectElement> Projects { get; set; }
    }
}
