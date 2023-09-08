namespace Shared.Models.Areas.Reports
{
    public class RetentionInfoInfo
    {
        public int RunID { get; set; } //ID
        public string DocLayout { get; set; } //Tipo de documento
        public string DocType { get; set; } //Tipo de documento
        public string DescDoc { get; set; } //Descrição
        public string ProductionStateID { get; set; } //Estado
        public int PostObjID  { get; set; } //Data/Hora
        public decimal PrintCost { get; set; } //Custo de Impressão
        public decimal MaterialCost { get; set; } //Custo de Materiais
        public string TemplateID { get; set; } //Template
        public string Product { get; set; } //Produto/Ramo
        public string Firm { get; set; } //Companhia
        public string Branch { get; set; } //Rede
        public string ClientSegment { get; set; } //Segmento de cliente
        public string Periodicity { get; set; } //Periodicidade
        public int CECode { get; set; } //Código CE
        public string ClientType { get; set;} //Tipo de cliente
        public string ClientNr { get; set; } //Nº Cliente
        public int DocSourceNr { get; set; } //Nº do Documento na Origem
        public string DocDescription { get; set; } //Descrição do Documento
        public int NIF { get; set; } //NIF
        public string PolicyNr { get; set; } //Nº da Apólice
        public int CertificateNr { get; set;} //Nº de Certificado
        public int SinisterProcessNr { get; set; } //Nº de Processo de Sinistro
        public string AccountProduct { get; set; } //Produto da conta
        public int AccountNr { get; set; } //Nº da Conta
        public string AccountPlan { get; set; } //Plano da Conta
        public string AccountSubProduct { get; set; } //SubProduto da Conta
        public string ReplaceFlag { get; set;} //Documento de substituição
        public int DocDate { get; set; } //Data do Documento
        public DateTime FiscalYear { get; set; } //Ano Fiscal
        public int CopyNr { get; set; } //Via
        public int CostCenter { get; set; } //Centro de Custo
        public string ExpPaymentFlag { get; set; } //Inclusão de Portes
        public int NIB { get; set; } //NIB
        public string AdressCompany { get; set; } //Companhia da Morada
        public string AdressProductAccount { get; set; } //Produto da Conta da Morada
        public int AdressAccountNr { get; set; } //Nº da Conta da Morada

        public int FileID { get; set; }
        public int SetID { get; set; }
        public int DocID { get; set; }
        public string AccountNrStr { get; set; }
    }
}
