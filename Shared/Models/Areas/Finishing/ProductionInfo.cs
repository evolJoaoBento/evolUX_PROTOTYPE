namespace Shared.Models.Areas.Finishing { 
    public class ProductionInfo
    {
        public int RunID { get; set; }
        public int FileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ShortFileName { get; set; }
        public string FilePrinterSpecs { get; set; }
        public IEnumerable<ResourceInfo> FilePrinters { get; set; }
        public string RegistDetailFileName { get; set; }
        public string RegistDetailShortFileName { get; set; }
        public string RegistDetailFilePrinterSpecs { get; set; }
        public string RegistShortFileName { get; set; }
        public IEnumerable<ResourceInfo> RegistDetailFilePrinters { get; set; }
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
        public string ExpeditionLevel { get; set; }
        public string ExpeditionZone { get; set; }
        public string ExpeditionType { get; set; }
        public bool FilePrintedFlag { get; set; }
        public bool RegistDetailFilePrintedFlag { get; set; }
        public Dictionary<string, int> PaperTotals { get; set; }
        public Dictionary<string, int> StationTotals { get; set; }
        public ProductionInfo()
        {
            FilePrinters = new List<ResourceInfo>();
            RegistDetailFilePrinters = new List<ResourceInfo>();
            //FilePath = "";
            //FileName = "";
            //ShortFileName = "";
            //FilePrinterSpecs = "";
            //RegistDetailFileName = "";
            //RegistDetailShortFileName = "";
            //RegistDetailFilePrinterSpecs = "";
            //RegistShortFileName = "";
            //RegistFilePrinterSpecs = "";
            //ServiceTaskCode = "";
            //PrinterOperator = "";
            //Printer = "";
            //PlexCode = "";
            //FullFillMaterialRef = "";
            //FullFillMaterialCode = "";
            //ExpCompanyCode = "";
            //ExpCenterCode = "";
            //ExpeditionLevel = "";
            //ExpeditionZone = "";
            //ExpeditionType = "";
            //PaperTotals = new Dictionary<string, int>();
            //StationTotals = new Dictionary<string, int>();
        }

    }
}
