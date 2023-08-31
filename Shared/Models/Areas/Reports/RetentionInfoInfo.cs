namespace Shared.Models.Areas.Reports
{
    public class RetentionInfoInfo
    {
        public string BusinessAreaID { get; set; }
        public DateTime DateRef { get; set; }
        public DateTime RunDate { get; set; }
        public string DocTypeLayout { get; set; }
        public string DocTypeSubtype { get; set; }
        public int RetentionDocs { get; set; }
        public DateTime MaxProduction { get; set; }
        public DateTime MaxRetention { get; set; }
        public int RunID { get; set; }
        public int DocID { get; set; }
        public string DocDescription { get; set; }
        public int NrDoc { get; set; }
        public string NrApolice { get; set; }
        public string Product { get; set; }
        public string DocState { get; set; }
        public string DocDate { get; set; }
        public decimal PrintCost { get; set; }
        public decimal MaterialsCost { get; set; }
        public string Template { get; set; }
        public string ProductBranch { get; set; }
        public string Company { get; set; }
        public string Network { get; set; }
        public string ClientSegment { get; set; }
        public string Periodicity { get; set; }
        public int CECode { get; set; }
        public string ClientType { get; set;}
        public int ClientNr { get; set; }
        public string DescDoc { get; set; }
        public int NIF { get; set; }
        public int CertificateNr { get; set;}
        public int ProcessNr { get; set; }
        public string AccountProduct { get; set; }
        public int AccountNr { get; set; }
        public string AccountPlan { get; set; }
        public string AccountSubProduct { get; set; }
        public string SubsDoc { get; set;}
        public DateTime DocumentDate { get; set; }
        public DateTime FiscalYear { get; set; }
        public int Via { get; set; }
        public int CostCenter { get; set; }
        public string Ports { get; set; }
        public int NIB { get; set; }
        public string AdressCompany { get; set; }
        public string AdressProduct { get; set; }
        public int AdressNr { get; set; }

    }
}
