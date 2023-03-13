using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class DocCodeViewModel: ItemPermissions
    {
        public IEnumerable<DocCode> DocCodeList { get; set; }
    }
}
