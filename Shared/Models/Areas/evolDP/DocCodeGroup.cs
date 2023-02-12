namespace Shared.Models.Areas.evolDP
{
    public class DocCodeGroup
    {
        public string DocLayout { get; set; }
        public List<DocCode> DocCodes { get; set; }
        public DocCodeGroup(string docLayout)
        {
            DocLayout = docLayout;
            DocCodes = new List<DocCode>();
        }
    }
}
