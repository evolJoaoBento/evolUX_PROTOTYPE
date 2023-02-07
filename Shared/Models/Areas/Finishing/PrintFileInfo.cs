using Shared.Models.Areas.Core;

namespace Shared.Models.Areas.Finishing
{
    public class PrintFileInfo
    {
        public int RunID { get; set; }
        public int FileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ShortFileName { get; set; }
        public string FilePrinterSpecs { get; set; }

        public int PrintRecNumber { get; set; }
        public PrintFileInfo()
        {
            FilePath = "";
            FileName = "";
            ShortFileName = "";
            FilePrinterSpecs = "";
            PrintRecNumber = -1;
        }

    }
}
