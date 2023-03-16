using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ConstantParameterViewModel: ItemPermissions
    {
        public IEnumerable<ConstantParameter> ConstantsList { get; set; }
    }
}
