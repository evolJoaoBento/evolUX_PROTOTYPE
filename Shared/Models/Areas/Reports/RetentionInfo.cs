namespace Shared.Models.Areas.Reports
{
    public class RetentionInfo
    {
        public string DocLayout { get; set; }
        public string DocType { get; set; }
        public int FileID { get; set; }
        public int ProdMaxDate { get; set; }
        public int SetID { get; set; }
        public int DocID { get; set; }
        public int RunDate { get; set; }
        public int RunID { get; set; }
        public string BusinessAreaID { get; set; }
        public string Firm { get; set; }
        public string Branch { get; set; }
        public string ClientSegment { get; set; }
        public int DocDate { get; set; }
        public string ClientNr { get; set; }
        public string AccountNrStr { get; set; }

        public string NrApolice { get; set; }
        public string Product { get; set; }


        //public int RetentionDocs { get; set; }
        //public DateTime MaxProduction { get; set; }
        //public DateTime MaxRetention { get; set; }
        //public DateTime RefDate { get; set; }
        //public string DocDescription { get; set; }
        

    }
}
