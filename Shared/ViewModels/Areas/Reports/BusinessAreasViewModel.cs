using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.Reports
{
    public class BusinessAreasViewModel : ItemPermissions
    {
        public IEnumerable<Business> BusinessAreas { get; set; }
    }
}
