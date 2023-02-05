namespace Shared.Models.Areas.Finishing
{
    public class ProdServiceElement
    {
        public string ServiceTaskCode { get; set; }
        public string ServiceTaskDec { get; set; }
        public List<ProdFullFillElement> FullFillList { get; set; }
        public ProdServiceElement()
        {
            FullFillList = new List<ProdFullFillElement>();
            ServiceTaskCode = string.Empty;
            ServiceTaskDec = string.Empty;
        }
    }
}
