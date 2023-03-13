namespace Shared.Models.Areas.evolDP
{
    public class DocCodeConfig
    {
        public int StartDate { get; set; }
        public int AggrCompatibility { get; set; }
        public int EnvMediaID { get; set; }
        public string EnvMediaDesc { get; set; }
        public int ExpeditionType { get; set; }
        public string ExpCode { get; set; }
        public int ExpCompanyID { get; set; }
        public string ExpCompanyName { get; set; }
        public int ServiceTaskID { get; set; }
        public string ServiceTaskCode { get; set; }
        public string ServiceTaskDesc { get; set; }
        public int SuportType { get; set; }
        public int Priority { get; set; }
        public string CaducityDate { get; set; }
        public string MaxProdDate { get; set; }
        public int? ProdMaxSheets { get; set; }
        public string ArchCaducityDate { get; set; }

        public bool IsEditable { get; set; }
    }
}
