using Shared.Models.Areas.Core;
using Shared.Models.Areas.Reports;
using System.Data;

namespace Shared.ViewModels.Areas.Reports
{
    public class DependentProductionViewModel : ItemPermissions
    {
        public DataTable ServiceCompanyList { get; set; }
        public IEnumerable<DependentPrintsInfo> DependentPrintProduction { get; set; }
        public IEnumerable<DependentFullfillInfo> DependentFullfillProduction { get; set; }
    }
}
