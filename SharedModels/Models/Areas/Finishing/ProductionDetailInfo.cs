namespace Shared.Models.Areas.Finishing
{
    public class ProductionDetailInfo
    {
        public int ServiceCompanyID { get; set; }
        public int RunID { get; set; }
        public int PaperMediaID { get; set; }
        public int StationMediaID { get; set; }
        public int ExpeditionType { get; set; }
        public string ExpCode { get; set; }
        public bool HasColorPages { get; set; }
        public IEnumerable<ProductionInfo> ProductionDetailReport { get; set; }
    }
}
