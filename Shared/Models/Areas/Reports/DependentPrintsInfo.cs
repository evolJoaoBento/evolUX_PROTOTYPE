namespace Shared.Models.Areas.Reports
{
    public class DependentPrintsInfo
    {
        public DateTime FileTimeStamp { get; set; } //Data/Hora da impressão
        public string BusinessDesc { get; set; } //Nome da área de negócio
        public string ExpeditionType { get; set; } //Tipo de expedição
        public string ExpCode { get; set; } //Companhia de expedição/tratamento
        public string PlexCode { get; set; } //Tipo de impressão
        public int TotalFiles { get; set; } //Ficheiros
        public int TotalDocs { get; set; } //Docs
        public int TotalPrint { get; set; } //Impressões
    }
}
