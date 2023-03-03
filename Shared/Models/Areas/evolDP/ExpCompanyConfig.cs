namespace Shared.Models.Areas.evolDP
{
    public class ExpCompanyConfig: ExpCompanyConfigResume
    {
        public int ExpCompanyLevel { get; set; }
		public int MaxWeight { get; set; } //Peso Máximo
        public double UnitCost { get; set; } //Custo Unitário
        public string ExpColumnA { get; set; } //Zona de Taxação
        public string ExpColumnB{ get; set; } //Produto
        public string ExpColumnE { get; set; } //Velocidade
        public string ExpColumnI { get; set; } //Serviços Especiais
        public string ExpColumnF { get; set; } 
    }
}