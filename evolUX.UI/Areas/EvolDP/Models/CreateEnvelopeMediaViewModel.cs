using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX_dev.Areas.EvolDP.Models
{
    public class CreateEnvelopeMediaViewModel
    {
        public string CompanyName { get; set; }
        public int ExpCompanyId { get; set; }
        public int ContractId { get; set; }
        public string ContractNumber { get; set; }
        public string ClientNr { get; set; }
        public string ClientName { get; set; }

        public string EnvMediaName { get; set; }
        public string EnvMediaDescription { get; set; }
        public string[] Media  { get; set; }

        public string[] ExpCompanyContract { get; set; }
        public List<DropListItemViewModel> MaterialList { get; set; }

        public List<DropListTwoValues> DropListTwoValues { get; set; }

    }

    public class DropListTwoValues
    {
        public string Caption1 { get; set; }
        public string Value1 { get; set; }
        public string Caption2 { get; set; }
        public string Value2 { get; set; }
        public string ExpType { get; set; }
        public string FFMCode { get; set; }
    }
}

