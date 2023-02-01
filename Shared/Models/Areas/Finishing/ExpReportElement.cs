namespace Shared.Models.Areas.Finishing
{
    public class ExpReportElement
    {
        public int ExpReportID { get; set; }
        public string ExpRegistReportID { get; set; }
        public int ExpReportNr { get; set; }
        public string ExpTimeDate { get; set; }
        public DateTime ExpTimeStamp { get; set; }
        public string ExpTime { get; set; }
        public ExpContractFileElement ExpContract { get; set; }
        public ExpReportElement()
        {
            ExpContract = new ExpContractFileElement();
            ExpTimeDate= string.Empty;
            ExpTime = string.Empty;
            ExpRegistReportID = string.Empty;
        }

    }
}
