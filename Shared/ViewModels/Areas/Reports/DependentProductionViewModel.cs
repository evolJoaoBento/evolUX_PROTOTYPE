using Shared.Models.Areas.Core;
using Shared.Models.Areas.Reports;

namespace Shared.ViewModels.Areas.Reports
{
    public class DependentProductionViewModel : ItemPermissions
    {
        public int RefDate { get; set; }
        public IEnumerable<DependentPrintsInfo> DependentPrintProduction { get; set; }
        public IEnumerable<DependentEnvsInfo> DependentEnvProduction { get; set; }
    }
}
