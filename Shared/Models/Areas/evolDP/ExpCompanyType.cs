namespace Shared.Models.Areas.evolDP
{
    public class ExpCompanyType
    {
        public int ExpCompanyID { get; set; }
        public int ExpeditionType { get; set; }
        public string ExpeditionTypeDesc { get; set; }
        public bool RegistMode { get; set; }
        public bool SeparationMode { get; set; }
        public bool? BarcodeRegistMode { get; set; }
        public bool GenerateDetailRegist { get; set; }
        public bool DRFlagRegistado { get; set; }
        public bool DRFlagEncomenda { get;set; }
        public bool DRFlagPessoal { get; set; }
        public int Priority { get; set; }
    }
}