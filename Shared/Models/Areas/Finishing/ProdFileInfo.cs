using Shared.Models.Areas.Core;

namespace Shared.Models.Areas.Finishing
{
    public class ProdFileInfo
    {
        public string PlexCode { get; set; }

        public int RunID { get; set; }
        public int FileID { get; set; }
       
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ShortFileName { get; set; }
        public string FilePrinterSpecs { get; set; }
        public bool FilePrintedFlag { get; set; }
        public int FileColor { get; set; }
        public int FilePlexType { get; set; }

        public string RegistDetailFileName { get; set; }
        public string RegistDetailShortFileName { get; set; }
        public string RegistDetailFilePrinterSpecs { get; set; }
        public bool RegistDetailFilePrintedFlag { get; set; }
        public int RegistDetailFileColor { get; set; }
        public int RegistDetailFilePlexType { get; set; }

        public int EnvMaterialID { get; set; }
        public string EnvMaterialRef { get; set; }

        public string PrinterOperator { get; set; }
        public string Printer { get; set; }
        public int TotalPrint { get; set; }
        public int StartSeqNum { get; set; }
        public int EndSeqNum { get; set; }
        public int TotalPostObjs { get; set; }

        public int ExpLevel { get; set; }
        public string ExpCenterCode { get; set; }
        public string ExpeditionLevel { get; set; }
        public string ExpeditionZone { get; set; }

        public Dictionary<string, int> PaperTotals { get; set; }
        public Dictionary<string, int> StationTotals { get; set; }
        public ProdFileInfo()
        {
            FilePath = "";
            FileName = "";
            ShortFileName = "";
            FilePrinterSpecs = "";
            RegistDetailFileName = "";
            RegistDetailShortFileName = "";
            RegistDetailFilePrinterSpecs = "";
            PrinterOperator = "";
            Printer = "";
            PlexCode = "";
            EnvMaterialRef = "";
            ExpCenterCode = "";
            ExpeditionLevel = "";
            ExpeditionZone = "";
            PaperTotals = new Dictionary<string, int>();
            StationTotals = new Dictionary<string, int>();
        }

    }
}
