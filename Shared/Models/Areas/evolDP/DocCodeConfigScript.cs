namespace Shared.Models.Areas.evolDP
{
    public class DocCodeConfigScript
    {
        public string DocLayout { get; set; }
        public string DocType { get; set; }
        public string ExceptionLevel1Code { get; set; }
        public string ExceptionLevel2Code { get; set; }
        public string ExceptionLevel3Code { get; set; }

        public string DocDescription { get; set; }
        public string PrintMatchCode { get; set; }
        
        public int AggrCompatibility { get; set; }
        public string EnvMediaDesc { get; set; }
        public int ExpeditionType { get; set; }
        public string ExpCode { get; set; }
        public int SuportType { get; set; }
        public int Priority { get; set; }
        public string CaducityDate { get; set; }
        public string MaxProdDate { get; set; }
        public int? ProdMaxSheets { get; set; }
        public string ArchCaducityDate { get; set; }
    }
}
