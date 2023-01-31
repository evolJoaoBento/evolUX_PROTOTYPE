namespace Shared.Models.Areas.Finishing
{
    public class ExpBusinessFileElement
    {
        public int CompanyID { get; set; }
        public int BusinessID { get; set; }
        public string BusinessCode { get; set; }
        public string BusinessDescription { get; set; }
        public List<ExpRunFileElement> RunList { get; set; }
        public ExpBusinessFileElement()
        {
            BusinessCode = string.Empty;
            BusinessDescription = string.Empty;
            RunList = new List<ExpRunFileElement>();
        }
    }
}
