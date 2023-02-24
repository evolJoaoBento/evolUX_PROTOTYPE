using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX_dev.Areas.evolDP.Models
{
    public class EditEnvMediaGroupForExpCompanyViewModel
    {
        public List<DropListItemViewModel> DropList { get; set; }
        public int EnvMediaGroupId { get; set; }
        public int ExpCompanyId { get; set; }
        public int EnvMediaId { get; set; }
        public string CompanyName { get; set; }
        public string GroupName { get; set; }
    }
}
