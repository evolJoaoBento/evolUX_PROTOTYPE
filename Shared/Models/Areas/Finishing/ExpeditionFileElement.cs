namespace Shared.Models.Areas.Finishing
{
    public class ExpeditionFileElement
    {
        public int CompanyID { get; set; }
        public int BusinessID { get; set; }
        public string BusinessCode { get; set; }
        public string BusinessDescription { get; set; }
        public int RunDate { get; set; }
        public int RunSequence { get; set; }
        public int RunID { get; set; }
        public int FileID { get; set; }
        public string FileName { get; set; }
        public int ExpCompanyID { get; set; }
        public string ExpCompanyCode { get; set; }
        public string ClientName { get; set; }
        public int ContractNr { get; set; }
        public int ClientNr { get; set; }
        public int ContractID { get; set; }
        public ExpeditionFileElement()
        {
            BusinessCode = string.Empty;
            BusinessDescription = string.Empty;
            FileName = string.Empty;
            ExpCompanyCode = string.Empty;
            ClientName = string.Empty;
        }
    }
}
