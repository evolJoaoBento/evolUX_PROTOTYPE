namespace Shared.Models.Areas.evolDP
{
    public class ExpCompanyConfig
    {
        public int ExpCompanyID { get; set; }
        public int ExpeditionZone { get; set; }
        public string ExpeditionZoneDesc { get; set; }
        public int ExpeditionType { get; set; }
        public string ExpeditionTypeDesc { get; set; }
        public int ExpCompanyLevel { get; set; }
		public int MaxWeight { get; set; } //Peso Máximo
        public int StartDate { get; set; } //Data de Efetivação
        public double UnitCost { get; set; } //Custo Unitário
        public string ExpColumnA { get; set; } //Zona de Taxação
        public string ExpColumnB{ get; set; } //Produto
        public string ExpColumnE { get; set; } //Velocidade
        public string ExpColumnI { get; set; } //Serviços Especiais
        public string ExpColumnF { get; set; } 
    }
}