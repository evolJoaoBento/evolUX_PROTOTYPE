namespace evolUX.API.Areas.Finishing.Models
{
    public class ProductionInfo
    {
        public int RunID { get; set; }
        public int FileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ShortFileName { get; set; }
        public string FilePrinterSpecs { get; set; }
        public string RegistShortFileName { get; set; }
        public string RegistFilePrinterSpecs { get; set; }
        public int ServiceTaskCode { get; set; }
        public string PrinterOperator { get; set; }
        public string Printer { get; set; }
        public int PlexCode { get; set; }
        public int TotalPrint { get; set; }
        public int StartSeqNum { get; set; }
        public int EndSeqNum { get; set; }
        public string FullFillMaterialRef { get; set; }
        public int TotalPostObjs { get; set; }
        public int ExpLevel { get; set; }
        public int ExpCompanyCode { get; set; }
        public string ExpCenterCodeDesc { get; set; }//SHOULDNT IT BE COMPANY?
        public string ExpeditionZone { get; set; }
        public Dictionary<string, int> PaperTotals { get; set; }
        public Dictionary<string, int> StationTotals { get; set; }
    }
}
