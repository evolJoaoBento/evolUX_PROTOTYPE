namespace Shared.Models.Areas.evolDP
{
    public class VersionElement
    {
        public int VersionID { get; set; }
        public int StartDate { get; set; }
        public int StartTime { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Revision { get; set; }
        public int Patch { get; set; }
        public string Description { get; set; }
    }
}
