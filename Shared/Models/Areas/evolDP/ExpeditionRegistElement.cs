namespace Shared.Models.Areas.evolDP
{
    public class ExpeditionRegistElement
    {
        public int ExpCompanyID { get; set; }
        public int StartExpeditionID { get; set; }
        public int EndExpeditionID { get; set; }
        public int CompanyRegistCode { get; set; }
        public int LastExpeditionID { get; set; }
        public string RegistCodePrefix { get; set; }
        public string RegistCodeSuffix { get; set; }
    }
}