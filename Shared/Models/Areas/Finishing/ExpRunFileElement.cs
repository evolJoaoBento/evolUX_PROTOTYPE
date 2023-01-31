namespace Shared.Models.Areas.Finishing
{
    public class ExpRunFileElement
    {
        public int RunID { get; set; }
        public int RunDate { get; set; }
        public int RunSequence { get; set; }
        public List<ExpContractFileElement> ContractList { get; set; }
        public ExpRunFileElement()
        {
            ContractList = new List<ExpContractFileElement>();
        }
    }
}
