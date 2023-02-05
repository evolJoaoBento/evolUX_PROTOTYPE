namespace Shared.Models.Areas.Finishing
{
    public class ProductionDetailInfo
    {
        public int RunID { get; set; }
        public int ServiceCompanyID { get; set; }
        public string ServiceCompanyCode { get; set; }
        public string ServiceCompanyName { get; set; }
        public int ExpCompanyID { get; set; }
        public string ExpCompanyCode { get; set; }
        public string ExpCompanyName { get; set; }
        public int ExpeditionType { get; set; }
        public string ExpeditionTypeDesc { get; set; }
        public string ExpCode { get; set; }
        public string ServiceTaskCode { get; set; }
        public string ServiceTaskDesc { get; set; }
        public int EnvMaterialID { get; set; }
        public string EnvMaterialRef { get; set; }
        public string FullFillMaterialCode { get; set; }
        public int FullFillCapacity { get; set; }
        public int PlexType { get; set; }
        public string PlexCode { get; set; }

        public int PaperMediaID { get; set; }
        public int StationMediaID { get; set; }
        public bool HasColorPages { get; set; }
        public IEnumerable<ProductionInfo> ProductionDetailReport { get; set; }
        public IEnumerable<ProdServiceCompanyElement> ProductionDetailPrinterReport { get; set; }
    }
}
