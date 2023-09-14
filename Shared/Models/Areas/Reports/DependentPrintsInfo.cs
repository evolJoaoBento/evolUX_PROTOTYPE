namespace Shared.Models.Areas.Reports
{
    internal class DependentPrintsInfo
    {
        public int EndTimeStamp { get; set; } //Data/Hora da impressão
        public int Description { get; set; } //Nome da área de negócio
        public int ExpeditionType { get; set; } //Tipo de expedição
        public int ExpCode { get; set; } //Companhia de expedição/tratamento
        public int PlexCode { get; set; } //Tipo de impressão
        public int FileID { get; set; } //Ficheiros
        public int TotalDocs { get; set; } //Docs
        public int TotalPrint { get; set; } //Impressões
    }
}
