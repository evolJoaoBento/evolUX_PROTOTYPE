using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ExpeditionTypeViewModel : ItemPermissions
    {
        public IEnumerable<ExpeditionTypeElement> Types { get; set; }
        public IEnumerable<Company> ExpCompanies { get; set; }
    }
}
