using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ExpCodeViewModel : ItemPermissions
    {
        public ExpCodeElement ExpCode { get; set; }
        public IEnumerable<ExpCenterElement> ExpCenters { get; set; } = Enumerable.Empty<ExpCenterElement>();
        public IEnumerable<Company> ServiceCompanies { get; set; } = Enumerable.Empty<Company>();
        public IEnumerable<ExpeditionZoneElement> Zones { get; set; }
    }
}
