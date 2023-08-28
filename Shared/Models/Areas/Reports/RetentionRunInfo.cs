namespace Shared.Models.Areas.Reports
{
    public class RetentionRunInfo
    {
        public string BusinessAreaID { get; set; }
        public DateTime DateRef { get; set; }
        public DateTime RunDate { get; set; }
        public string DocTypeLayout { get; set; }
        public string DocTypeSubtype { get; set; }
        public int RetentionDocs { get; set; }
        public DateTime MaxProduction { get; set; }
        public int RunID { get; set; }

    }
}
