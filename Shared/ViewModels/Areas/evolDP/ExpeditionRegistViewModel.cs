using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ExpeditionRegistViewModel : ItemPermissions
    {
        public Company Company { get; set; }
        public IEnumerable<ExpeditionRegistElement> ExpeditionRegistIDs { get; set; }
    }
}
