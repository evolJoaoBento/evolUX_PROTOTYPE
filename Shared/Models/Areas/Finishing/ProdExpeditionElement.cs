namespace Shared.Models.Areas.Finishing
{
    public class ProdExpeditionElement
    {
        public int ExpCompanyID { get; set; }
        public string ExpCompanyCode { get; set; }
        public string ExpCompanyName { get; set; }
        public int ExpeditionType { get; set; }
        public string ExpeditionTypeDesc { get; set; }
        public List<ProdServiceElement> ServiceList { get; set; }
        public ProdExpeditionElement()
        {
            ServiceList = new List<ProdServiceElement>();
            ExpCompanyCode = string.Empty;
            ExpCompanyName = string.Empty;
            ExpeditionTypeDesc = string.Empty;
        }
    }
}
