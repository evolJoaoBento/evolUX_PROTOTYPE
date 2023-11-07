using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ViewModels.Areas.evolDP
{
    public class MaterialCostViewModel : ItemPermissions
    {
        public string MaterialTypeCode { get; set; }
        public MaterialElement Material { get; set; }
        public IEnumerable<FullfillMaterialCode> FullfillMaterialCodes { get; set; }
        public IEnumerable<ServiceCompanyRestriction> Restrictions { get; set; }
        public IEnumerable<Company> ServiceCompanies { get; set; }
        public IEnumerable<Company> Companies { get; set; }
    }
}

