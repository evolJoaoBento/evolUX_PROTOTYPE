using evolUX.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX.API.Areas.EvolDP.Models
{
    public class DocCode
    {
        //TODO: CLEANUP
        public int DocCodeID { get; set; }
        public string DocLayout { get; set; }
        public string DocType { get; set; }
        public string DocDescription { get; set; }
        public DocException DocExceptionLevel1 { get; set; }
        public DocException DocExceptionLevel2 { get; set; }
        public DocException DocExceptionLevel3 { get; set; }
        //public EnvMediaGroupViewModel EnvMediaGroup { get; set; }
        //public int DocStartDate { get; set; }
        //public string DocEnvelopeGroup { get; set; }
        //public string DocAggregation { get; set; }
        //public int DocPriority { get; set; }
        //public string DocNMaxPaper { get; set; }
        //public Company DocExpeditionCompany { get; set; }
        //public ExpeditionType DocExpeditionType { get; set; }
        //public string DocTreatmentType { get; set; }
        //public string DocPaperProduction { get; set; }
        //public DateTime DocProductionExpiryDate { get; set; }
        //public string DocProductionMaxDate { get; set; }
        //public string DocToFileExpiryDate { get; set; }
        //public string DocToFile { get; set; }
        //public SuportTypeReference DocEmailSend { get; set; }
        //public string DocColorFile { get; set; }
        //public string DocElectronicFormat { get; set; }
        //public string DocDigitalSignPDF { get; set; }

    }
}
