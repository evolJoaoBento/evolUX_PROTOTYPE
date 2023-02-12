namespace Shared.Models.Areas.evolDP
{
    public class DocCodeConfig
    {
        public int StartDate { get; set; }
        public string AggrCompatibility { get; set; }
        public string EnvMedia { get; set; }
        public string EnvMediaDesc { get; set; }
        public string ExpeditionType { get; set; }
        public string ExpCode { get; set; }
        public int ExpCompanyID { get; set; }
        public string ExpCompanyName { get; set; }
        public int ServiceTaskID { get; set; }
        public string ServiceTaskCode { get; set; }
        public string ServiceTaskDesc { get; set; }
        public int SupotType { get; set; }
        public string Finishing { get; set; }
        public string Archive { get; set; }
        public string Electronic { get; set; }
        public string ElectronicHide { get; set; }
        public string Email { get; set; }
        public string EmailHide { get; set; }
        public string Priority { get; set; }
        public string CaducityDate { get; set; }
        public string MaxProdDate { get; set; }
        public string ProdMaxSheets { get; set; }
        public string ArchCaducityDate { get; set; }

        public bool IsEditable { get; set; }
    }
}
