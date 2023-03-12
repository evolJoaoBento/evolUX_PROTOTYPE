namespace Shared.Models.Areas.evolDP
{
    public class ServiceTypeElement
    {
        public int ServiceTypeID { get; set; }
        public string ServiceTypeCode { get; set; }
        public string ServiceTypeDesc { get; set; }
        public List<ServiceElement> ServicesList { get; set; } = new List<ServiceElement>();
    }
}
