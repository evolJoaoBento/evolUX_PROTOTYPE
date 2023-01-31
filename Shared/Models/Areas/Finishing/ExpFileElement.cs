namespace Shared.Models.Areas.Finishing
{
    public class ExpFileElement
    {
        public int RunID { get; set; }
        public int FileID { get; set; }
        public string FileName { get; set; }
        public ExpFileElement()
        {
            FileName = "";
        }
        public ExpFileElement(int runID, int fileID)
        {
            RunID = runID;
            FileID = fileID;
            FileName = "";
        }
    }
}
