using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ExceptionLevelViewModel: ItemPermissions
    {
        public int Level { get; set; }
        public IEnumerable<ExceptionLevel> ExceptionslevelList { get; set; }
    }
}
