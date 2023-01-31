namespace Shared.Models.Areas.Finishing
{
    public class ExpCompanyFileElement
    {
        public int ExpCompanyID { get; set; }
        public string ExpCompanyCode { get; set; }
        public List<ExpBusinessFileElement> BusinessList { get; set; }
        public ExpCompanyFileElement()
        {
            BusinessList = new List<ExpBusinessFileElement>();
            ExpCompanyCode = string.Empty;
        }
    }
}
