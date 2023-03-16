using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ExpContractViewModel : ItemPermissions
    {
        public Company Company { get; set; }
        public IEnumerable<ExpContractElement> ExpeditionContracts { get; set; }
    }
}
