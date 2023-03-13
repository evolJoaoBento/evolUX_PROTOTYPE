using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX.API.Models
{
    public class ServiceCompany
    {
        public int ServiceCompanyID { get; set; }
        public string ServiceCompanyName { get; set; }
        public string MaterialSupplier { get; set; }
        public string EffectiveDate { get; set; }
        public string MaterialCost { get; set; }
        public int MaterialPosition { get; set; }
        public Material Material { get; set; }

    }
}
