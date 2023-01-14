using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX.API.Models
{
    public class EditEnvMediaGroupForExpCompanyViewModel
    {
        public List<DropListItemViewModel> DropList { get; set; }
        public int EnvMediaGroupID { get; set; }
        public int ExpCompanyID { get; set; }
        public int EnvMediaID { get; set; }
        public string CompanyName { get; set; }
        public string GroupName { get; set; }
    }
}
