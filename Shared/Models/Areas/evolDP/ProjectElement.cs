namespace Shared.Models.Areas.evolDP
{
    public class ProjectElement
    {
        public int BusinessID { get; set; }
        public string BusinessCode { get; set; }
        public string BusinessDescription { get; set; }
        public string Type { get; set; }
        public List<Project> ProjectList { get; set; } = new List<Project>();
    }
}
