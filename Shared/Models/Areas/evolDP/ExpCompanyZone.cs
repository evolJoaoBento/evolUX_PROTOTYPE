namespace Shared.Models.Areas.evolDP
{
    public class ExpCompanyZone
    {
        public int ExpCompanyID { get; set; }
        public int ExpeditionZone { get; set; }
        public int ServiceTaskID { get; set; }
        public string ServiceTaskDesc { get; set; }
        public int Priority { get; set; }
        public string ExpCenterCode { get; set; }
        public string ServiceCompanyName { get; set; }
        public int ServiceCompanyID { get;set; }
    }
}
