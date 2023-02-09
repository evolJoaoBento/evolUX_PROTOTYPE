using Shared.Models.Areas.Core;

namespace Shared.Models.Areas.Finishing
{
    public class FileBase
    {
        public int RunID { get; set; }
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public FileBase()
        {
            FileName = "";
            FilePath = "";
        }
        public FileBase(int runID, int fileID)
        {
            RunID = runID;
            FileID = fileID;
            FileName = "";
            FilePath = "";
        }
    }
}
