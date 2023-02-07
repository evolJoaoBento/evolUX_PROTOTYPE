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
        public int FileRecNumber { get; set; }

        public PrintFileInfo()
        {
            RunID = 0;
            FileID = 0;
            FilePath = "";
            FileName = "";
            ShortFileName = "";
            FilePrinterSpecs = "";
            FileRecNumber = -1;
        }

    }
}
