namespace Shared.Models.Areas.Reports
{
    public class RetentionRunInfo
    {
        public int RunID { get; set; }
        public string BusinessAreaID { get; set; }
        public int RunDate { get; set; }
        public string BusinessName { get; set; }
        public string DocTypeLayout { get; set; }
        public string DocTypeSubtype { get; set; }
        public int MaxProduction { get; set; }


        //public string DocDescription { get; set; }
        //public DateTime DateRef { get; set; }
        public int RetentionDocs { get; set; }
        //public DateTime MaxRetention { get; set; }
        //public int DocID { get; set; }
        //public int NrDoc { get; set; }
        //public string NrApolice { get; set; }
        //public string Product { get; set; }

    }
}
