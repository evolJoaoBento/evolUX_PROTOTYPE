using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class PrinterViewModel: ItemPermissions
    {
        public IEnumerable<PrinterInfo> Printers { get; set; }
    }
}
