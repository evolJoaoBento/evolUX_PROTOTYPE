namespace Shared.Models.Areas.Finishing
{
    public class ExpContractFileElement
    {
        public string ClientName { get; set; }
        public int ContractNr { get; set; }
        public int ClientNr { get; set; }
        public int ContractID { get; set; }
        public List<ExpFileElement> FileList { get; set; }
        public ExpContractFileElement()
        {
            FileList = new List<ExpFileElement>();
            ClientName = string.Empty;
        }
    }
}
