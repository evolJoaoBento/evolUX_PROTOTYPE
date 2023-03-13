namespace Shared.Models.Areas.evolDP
{
    public class ExpContractElement
    {
        public int ExpCompanyID { get; set; }
        public int ContractID { get; set; }
        public int ContractNr { get; set; }
        public int ClientNr { get; set; }
        public string ClientName { get; set; }
        public string ClientNIF { get; set; }
        public string ClientAddress { get; set; }
        public string CompanyExpeditionCode { get; set; }
        public string ClientPostalCode { get; set; }
        public string ClientPostalCodeDescription { get; set; }
        public decimal PurchaseOrderNr { get; set; }
    }
}