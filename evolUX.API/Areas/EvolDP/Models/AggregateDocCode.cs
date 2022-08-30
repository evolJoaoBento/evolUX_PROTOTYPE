using evolUX.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX.API.Areas.EvolDP.Models
{
    public class AggregateDocCode
    {
        public int DocCodeID { get; set; }
        public string DocLayout { get; set; }
        public string DocType { get; set; }
        public string DocDescription { get; set; }
        public DocException DocExceptionLevel1 { get; set; }
        public DocException DocExceptionLevel2 { get; set; }
        public DocException DocExceptionLevel3 { get; set; }
        public string Campatible { get; set; }
        public string CheckStatus { get; set; }
        public string AggrCompatibility { get; set; }
    }
}
