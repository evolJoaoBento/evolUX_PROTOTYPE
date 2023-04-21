using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ViewModels.Areas.evolDP
{
    public class MaterialTypeViewModel : ItemPermissions
    {
        public IEnumerable<MaterialType> MaterialTypeList { get; set; }
    }
}

