using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ExpCompanyViewModel : ItemPermissions
    {
        public Company ExpCompany { get; set; }
        public IEnumerable<ExpCompanyType> ExpTypes { get; set; }
        public IEnumerable<ExpCompanyConfigResume> Configs { get; set; }
        public IEnumerable<ExpeditionTypeElement> Types { get; set; }
    }
}
