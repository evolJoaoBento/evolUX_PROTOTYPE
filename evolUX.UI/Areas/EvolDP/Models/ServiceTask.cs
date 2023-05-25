using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX_dev.Areas.evolDP.Models
{
    public class ServiceTask
    {
        public int ServiceTaskId { get; set; }
        public string ServiceTaskCode { get; set; }
        public string ServiceTaskDescription { get; set; }
        public string StationExceededDesc { get; set; }
        public int ComplementServiceTaskId { get; set; }
    }
}
