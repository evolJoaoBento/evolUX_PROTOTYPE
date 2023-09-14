namespace Shared.Models.Areas.Reports
{
    internal class DependentEnvsInfo
    {
        public int EndTimeStamp { get; set; } //Data/Hora da impressão
        public int Description { get; set; } //Nome da área de negócio
        public int ExpeditionType { get; set; } //Tipo de expedição
        public int ExpCode { get; set; } //Companhia de expedição/tratamento
        public int FullFillMaterialCode { get; set; } //Formato de envelope
        public int MyProperty { get; set; } //Tipo de envelopagem
        public int FileID { get; set; } //Ficheiros
        public int TotalDocs { get; set; } //Docs
        public int TotalPostObjs { get; set; } //Envelopagens
    }
}
