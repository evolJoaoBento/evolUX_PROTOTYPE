using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ExpCompanyConfigViewModel : ItemPermissions
    {
        public ExpCompanyViewModel expCompanyView { get; set; }
        public IEnumerable<ExpeditionZoneElement> Zones { get; set; }
        public int ExpeditionType { get; set; }
        public int ExpeditionZone { get; set; }
        public int StartDate { get; set; }
    }
}
