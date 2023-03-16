namespace Shared.Models.Areas.evolDP
{
    public class ServiceTask
    {
        public int ServiceTaskID { get; set; }
        public string ServiceTaskCode { get; set; }
        public string ServiceTaskDesc { get; set; }
        public string StationExceededDesc { get; set; }
        public int ComplementServiceTaskID { get; set; }
        public int ExternalExpeditionMode { get; set; }
        public IEnumerable<ServiceTypeElement> ServiceTypes { get; set; }
    }
}
