﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX.API.Models
{
    public class ServiceTask
    {
        public int ServiceTaskID { get; set; }
        public string ServiceTaskCode { get; set; }
        public string ServiceTaskDescription { get; set; }
        public string StationExceededDesc { get; set; }
        public int ComplementServiceTaskID { get; set; }
    }
}
