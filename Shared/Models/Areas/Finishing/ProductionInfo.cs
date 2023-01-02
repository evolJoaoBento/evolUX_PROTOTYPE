namespace Shared.Models.Areas.Finishing { 
    public class ProductionInfo
    {
        public int RunID { get; set; }
        public int FileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ShortFileName { get; set; }
        public string FilePrinterSpecs { get; set; }
        public string RegistDetailFileName { get; set; }
        public string RegistDetailShortFileName { get; set; }
        public string RegistDetailFilePrinterSpecs { get; set; }
        public string RegistShortFileName { get; set; }
        public string RegistFilePrinterSpecs { get; set; }
        public string ServiceTaskCode { get; set; }
        public string PrinterOperator { get; set; }
        public string Printer { get; set; }
        public string PlexCode { get; set; }
        public int TotalPrint { get; set; }
        public int StartSeqNum { get; set; }
        public int EndSeqNum { get; set; }
        public string FullFillMaterialRef { get; set; }
        public string FullFillMaterialCode { get; set; }
        public int TotalPostObjs { get; set; }
        public int ExpLevel { get; set; }
        public string ExpCompanyCode { get; set; }
        public string ExpCenterCode { get; set; }
        public string ExpCenterCodeDesc { get; set; }
        public string ExpeditionZone { get; set; }
        public string ExpeditionType { get; set; }
        public bool FilePrintedFlag { get; set; }
        public bool RegistDetailFilePrintedFlag { get; set; }
        public Dictionary<string, int> PaperTotals { get; set; }
        public Dictionary<string, int> StationTotals { get; set; }
        
    }
}
