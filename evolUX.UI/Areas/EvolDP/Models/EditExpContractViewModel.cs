using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX_dev.Areas.EvolDP.Models
{
    public class EditExpContractViewModel
    {
        public int ExpCompanyId { get; set; }
        public int ContractId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public int ContractNumber { get; set; }
        public int ClientNumber { get; set; }
        public string ClientName { get; set; }
        public string ClientNIF { get; set; }
        public string ClientAddress { get; set; }
    }
}
