using Shared.Models.Areas.Core;

namespace Shared.Models.Areas.Finishing
{
    public class ProdServiceCompanyElement
    {
        public int ServiceCompanyID { get; set; }
        public string ServiceCompanyCode { get; set; }
        public string ServiceCompanyName { get; set; }
        public List<ProdExpeditionElement> ExpeditionList { get; set; }
        public ProdServiceCompanyElement()
        {
            ExpeditionList = new List<ProdExpeditionElement>();
            ServiceCompanyCode = string.Empty;
            ServiceCompanyName = string.Empty;
        }
    }
}
