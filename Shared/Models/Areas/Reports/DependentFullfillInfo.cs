namespace Shared.Models.Areas.Reports
{
    public class DependentFullfillInfo
    {
        public int FileTimeStamp { get; set; } //Data/Hora da impressão
        public int BusinessDesc { get; set; } //Nome da área de negócio
        public int ExpeditionType { get; set; } //Tipo de expedição
        public int ExpCode { get; set; } //Companhia de expedição/tratamento
        public int FullFillMaterialCode { get; set; } //Formato de envelope
        public int ServiceFullFillMaterialCode { get; set; } //Tipo de envelopagem
        public int TotalFiles { get; set; } //Ficheiros
        public int TotalDocs { get; set; } //Docs
        public int TotalPostObjs { get; set; } //Envelopagens
    }
}
