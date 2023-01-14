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
        public ExceptionLevel ExceptionLevel1 { get; set; }
        public ExceptionLevel ExceptionLevel2 { get; set; }
        public ExceptionLevel ExceptionLevel3 { get; set; }

    }
}
