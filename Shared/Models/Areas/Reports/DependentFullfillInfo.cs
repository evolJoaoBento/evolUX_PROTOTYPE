namespace Shared.Models.Areas.Reports
{
    public class DependentFullfillInfo
    {
        public DateTime FileTimeStamp { get; set; } //Data/Hora da impressão
        public string BusinessDesc { get; set; } //Nome da área de negócio
        public string ExpeditionType { get; set; } //Tipo de expedição
        public string ExpCode { get; set; } //Companhia de expedição/tratamento
        public string FullFillMaterialCode { get; set; } //Formato de envelope
        public string ServiceFullFillMaterialCode { get; set; } //Tipo de envelopagem
        public int TotalFiles { get; set; } //Ficheiros
        public int TotalDocs { get; set; } //Docs
        public int TotalPostObjs { get; set; } //Envelopagens
    }
}
