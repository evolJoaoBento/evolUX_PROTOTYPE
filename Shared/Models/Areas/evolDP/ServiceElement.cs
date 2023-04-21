namespace Shared.Models.Areas.evolDP
{
    public class ServiceElement
    {
        public int ServiceID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceDesc { get; set; }
        public int ServiceTypeID { get; set; }
        public string ServiceTypeDesc { get; set; }
        public string MatchCode { get; set; }
        public IEnumerable<int> CompanyList { get; set; } = new List<int>(); 
    }
}
