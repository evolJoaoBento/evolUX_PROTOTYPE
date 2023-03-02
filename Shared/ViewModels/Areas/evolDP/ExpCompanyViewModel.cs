using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ExpCompanyViewModel : ItemPermissions
    {
        public IEnumerable<ExpeditionTypeElement> Types { get; set; }
        public Company ExpCompany { get; set; }
        public IEnumerable<ExpCompanyConfig> Configs { get; set; }
    }
}
