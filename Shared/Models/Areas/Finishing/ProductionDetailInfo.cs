namespace Shared.Models.Areas.Finishing
{
    public class ProductionDetailInfo
    {
        public int ExpCompanyID { get; set; }
        public string ExpCompanyCode { get; set; }
        public string ExpCompanyName { get; set; }
        public int ExpeditionType { get; set; }
        public string ExpeditionTypeDesc { get; set; }
        public string ExpCode { get; set; }
        public int ServiceTaskID { get; set; }
        public string ServiceTaskCode { get; set; }
        public string ServiceTaskDesc { get; set; }
        public int PlexType { get; set; }
        public string PlexCode { get; set; }

        public int PaperMediaID { get; set; }
        public string PaperMaterialList { get; set; }
        public int StationMediaID { get; set; }
        public string StationMaterialList { get; set; }
        public bool HasColorPages { get; set; }
        public int ExpeditionPriority { get; set; }
        public int ExpCodePriority { get; set; }
        public IEnumerable<ProductionInfo> ProductionDetailReport { get; set; }
        public IEnumerable<ProdServiceCompanyElement> ProductionDetailPrinterReport { get; set; }
    }
}
