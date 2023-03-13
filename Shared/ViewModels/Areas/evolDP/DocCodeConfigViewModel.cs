using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class DocCodeConfigViewModel: ItemPermissions
    {
        public DocCode DocCode { get; set; }
        public GenericOptionList SuportTypeList { get; set; }
    }
}
