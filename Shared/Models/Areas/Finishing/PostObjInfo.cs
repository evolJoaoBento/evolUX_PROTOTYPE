namespace Shared.Models.Areas.Finishing
{
    public class PostObjInfo
    {
        public int PostObjRunID { get; set; }
        public int PostObjFileID { get; set; }
        public int PostObjID { get; set; }
        public string FileName { get; set; }
        public int SequenceNumber { get; set; }
        public int FirstSheetSequenceNumber { get; set;}
        public string Error { get; set; }
    }
}
