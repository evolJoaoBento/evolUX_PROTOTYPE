using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class ExpeditionListViewModel: ItemPermissions
    {
        public IEnumerable<ExpServiceCompanyFileElement> ExpeditionList { get; set; }
    }
}
