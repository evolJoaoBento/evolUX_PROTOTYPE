using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX_dev.Areas.EvolDP.Models
{
    public class DocCodeConfig
    {
        public string DocCodeId { get; set; }
        public string DocLayout { get; set; }
        public string DocType { get; set; }
        public string DocDescription { get; set; }
        public string DocStartDate { get; set; }
        public string DocAggregation { get; set; }
        public string DocEnvMediaID { get; set; }
        public string DocExpeditionType { get; set; }
        public string DocExpCompany { get; set; }
        public string DocServiceTask { get; set; }
        public string DocFinishing { get; set; }
        public string DocArchive { get; set; }
        public string DocElectronic { get; set; }
        public string DocElectronicHide { get; set; }
        public string DocEmail { get; set; }
        public string DocEmailHide { get; set; }
        public string DocPriority { get; set; }
        public string DocAgging { get; set; }
        public string DocArchCaducityDate { get; set; }
        public string DocCaducityDate { get; set; }
        public string DocMaxProdDate { get; set; }
        public string DocMaxProdSheets { get; set; }
    }
}
