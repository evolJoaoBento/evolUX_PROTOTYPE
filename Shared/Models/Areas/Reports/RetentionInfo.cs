namespace Shared.Models.Areas.Reports
{
    public class RetentionInfo
    {
        public string BusinessAreaID { get; set; }
        public DateTime DateRef { get; set; }
        public DateTime RunDate { get; set; }
        public string DocTypeLayout { get; set; }
        public string DocTypeSubtype { get; set; }
        public int RetentionDocs { get; set; }
        public DateTime MaxProduction { get; set; }
        public DateTime MaxRetention { get; set; }
        public int RunID { get; set; }
        public int DocID { get; set; }
        public string DocDescription { get; set; }
        public int NrDoc { get; set; }
        public string NrApolice { get; set; }
        public string Product { get; set; }

    }
}
