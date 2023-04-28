namespace Shared.Models.Areas.evolDP
{
    public class ExpCodeElement
    {
        public string ExpCode { get; set; }
        public int ServiceTaskID { get; set; }
        public int ExpCompanyID { get; set; }
        public string DefaultExpCenterCode { get; set; }
        public int DefaultExpCompanyZone { get; set; }
        public int Priority { get; set; }
        public string ExpCodeDesc { get; set; }
        public bool CheckExpCompanySepCodes { get; set; }
	    public int PostalCodeStart { get; set; }
    }
}
