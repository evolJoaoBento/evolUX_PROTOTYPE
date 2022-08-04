using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX.API.Areas.EvolDP.Models
{
    public class DocException
    {
        public int ExceptionLevelID { get; set; }
        public string ExceptionCode { get; set; }
        public string ExceptionDescription { get; set; }
    }
}
