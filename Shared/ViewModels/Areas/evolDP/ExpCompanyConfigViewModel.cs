using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ExpCompanyConfigViewModel : ItemPermissions
    {
        public Company ExpCompany { get; set; }
        public int ExpeditionType { get; set; }
        public int ExpeditionZone { get; set; }
        public IEnumerable<ExpCompanyConfig> Configs { get; set; }
        public IEnumerable<ExpeditionZoneElement> Zones { get; set; }
    }
}
