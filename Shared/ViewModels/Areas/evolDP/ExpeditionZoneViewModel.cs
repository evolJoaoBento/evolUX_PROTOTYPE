using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ExpeditionZoneViewModel : ItemPermissions
    {
        public IEnumerable<ExpeditionZoneElement> Zones { get; set; }
        public IEnumerable<Company> ExpCompanies { get; set; }
    }
}
