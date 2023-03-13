using Shared.Models.Areas.evolDP;

namespace Shared.Models.Areas.Finishing
{
    public class PrintFileInfo: FileBase
    {
        public string ShortFileName { get; set; }
        public string FilePrinterSpecs { get; set; }

        public int PrintRecNumber { get; set; }
        public PrintFileInfo()
        {
            ShortFileName = "";
            FilePrinterSpecs = "";
            PrintRecNumber = -1;
        }

    }
}
