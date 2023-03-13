using System;

namespace Shared.Models.Areas.Core
{
    public class Job
    {
        public int JobID { get; set; }
        public int RunID { get; set; }
        public int Priority { get; set; }
        public int FlowID { get; set; }
        public DateTime RegistrationTimeStamp { get; set; }
        public DateTime StartTimeStamp { get; set; }
        public DateTime EndTimeStamp { get; set; }
        public string JobDescription { get; set; }
        public string JobFactor { get; set; }
        public string Obs { get; set; }
        public string JobStatus { get; set; }
        public string StateDescription { get; set; }
    }
}
