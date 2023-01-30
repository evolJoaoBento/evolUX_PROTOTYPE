namespace Shared.Models.Areas.evolDP
{
    public class Business
    {
        public int BusinessID { get; set; }
        public string BusinessCode { get; set; }
        public int CompanyID { get; set; }
        public string Description { get; set; }
        public int FileSheetsCutoffLevel { get; set; }
        public int InternalExpeditionMode { get; set; }
        public int InternalCodeStart { get; set; }
        public int InternalCodeLen { get; set; }
        public int ExternalExpeditionMode { get; set; }
        public int TotalBannerPages { get; set; }
        public int PostObjOrderMode { get; set; }
    }
}
