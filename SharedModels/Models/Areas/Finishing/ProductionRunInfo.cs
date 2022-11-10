namespace SharedModels.Models.Areas.Finishing
{
    public class ProductionRunInfo
    {
        public int RunID { get; set; }
        public int ServiceCompanyID { get; set; }
        public string ServiceCompany { get; set; }
        public int BusinessID { get; set; }
        public string BusinessDesc { get; set; }
        public int RunDate { get; set; }
        public int RunSequence { get; set; }
        public int FilesLeftToPrint { get; set; }
        public int RecFilesLeftToPrint { get; set; }
        public ProductionRunDetail Processed { get; set; }
        public ProductionRunDetail ToPrint { get; set; }
        public ProductionRunDetail S2Printer { get; set; }
        public ProductionRunDetail Printed { get; set; }
        public ProductionRunDetail FullFill { get; set; }
        public ProductionRunDetail Expedition { get; set; }
    }
}
