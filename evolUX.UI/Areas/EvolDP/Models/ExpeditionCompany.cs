using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX_dev.Areas.EvolDP.Models
{
    public class ExpeditionCompany
    {
        public int ExpeditionCompanyId { get; set; }
        public string ExpeditionCompanyCode { get; set; }
        public string ExpeditionCompanyName { get; set; }
        public string ExpeditionCompanyAddress { get; set; }
        public string ExpeditionCompanyPostalCode{ get; set; }
        public string ExpeditionCompanyCountry{ get; set; }
    }
}
