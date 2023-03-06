namespace Shared.Models.Areas.evolDP
{
    public class ExpCompanyConfigResume
    {
        public int ExpCompanyID { get; set; }
        public int StartDate { get; set; } //Data de Efetivação
        public int ExpeditionZone { get; set; }
        public string ExpeditionZoneDesc { get; set; }
        public int ExpeditionType { get; set; }
        public string ExpeditionTypeDesc { get; set; }
    }
}