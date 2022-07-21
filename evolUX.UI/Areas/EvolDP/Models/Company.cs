using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX_dev.Areas.EvolDP.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPostalCode { get; set; }
        public string CompanyPostalCodeDescription { get; set; }
        public string CompanyCountry { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyServer { get; set; }
    }
}
