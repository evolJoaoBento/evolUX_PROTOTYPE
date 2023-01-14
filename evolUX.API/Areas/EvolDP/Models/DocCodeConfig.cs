using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX.API.Areas.EvolDP.Models
{
    public class DocCodeConfig
    {
        public string DocCodeID { get; set; }
        public string DocLayout { get; set; }
        public string DocType { get; set; }
        public ExceptionLevel DocExceptionLevel1 { get; set; }
        public ExceptionLevel DocExceptionLevel2 { get; set; }
        public ExceptionLevel DocExceptionLevel3 { get; set; }
        public string DocDescription { get; set; }
        public string PrintMatchCode { get; set; }
        public string StartDate { get; set; }
        public string EnvMedia { get; set; }
        public string AggrCompatibility { get; set; }
        public string Priority { get; set; }
        public string ProdMaxSheets { get; set; }
        public string CompanyName { get; set; }
        public string ExpeditionType { get; set; }
        public string TreatmentType { get; set; }
        public string Finishing { get; set; }
        public string CaducityDate { get; set; }
        public string MaxProdDate { get; set; }
        public string Archive { get; set; }
        public string ArchCaducityDate { get; set; }
        public string Email { get; set; }
        public string EmailHide { get; set; }
        public string Electronic { get; set; }
        public string ElectronicHide { get; set; }
        
        public string DocMessage { get; set; }
    }
}
