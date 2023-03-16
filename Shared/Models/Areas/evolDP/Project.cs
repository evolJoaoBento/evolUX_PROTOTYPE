namespace Shared.Models.Areas.evolDP
{
    public class Project
    {
        public string ProjectCode { get; set; }
        public List<VersionElement> VersionList { get; set; } = new List<VersionElement>();
    }
}
