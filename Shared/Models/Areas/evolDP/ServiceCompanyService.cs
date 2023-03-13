namespace Shared.Models.Areas.evolDP
{
    public class ServiceCompanyService
    {
        public int ServiceCompanyID { get; set; }
        public int ServiceTypeID { get; set; }
        public string ServiceTypeDesc { get; set; }
        public int ServiceID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceDesc { get; set; }
        public int CostDate { get; set; }
        public double ServiceCost { get; set; }
        public string Formula { get; set; }
        public string MatchCode { get; set; }
    }
}
