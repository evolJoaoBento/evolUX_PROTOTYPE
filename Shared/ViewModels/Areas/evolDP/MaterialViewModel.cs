using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ViewModels.Areas.evolDP
{
    public class MaterialViewModel : ItemPermissions
    {
        public string MaterialTypeCode { get; set; }
        public MaterialElement Group { get; set; }
        public IEnumerable<MaterialElement> MaterialList { get; set; }
        public IEnumerable<MaterialType> MaterialTypeList { get; set; }
        public IEnumerable<FullfillMaterialCode> FullfillMaterialCodes { get;set; }
        public IEnumerable<Company> ServiceCompanies { get; set; }
        public IEnumerable<Company> Companies { get; set; }
    }
}

