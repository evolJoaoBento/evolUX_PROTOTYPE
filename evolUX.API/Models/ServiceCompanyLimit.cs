using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX.API.Models
{
    public class ServiceCompanyLimit
    {
        public string ServiceCampaingCode { get; set; }
        public string MaterialType { get; set; }
        public int MaxAvailableStations { get; set; }
        public string MaxSheetLevelPerFile { get; set; }
        public string ActionInCaseOfOverfill { get; set; }
    }
}
