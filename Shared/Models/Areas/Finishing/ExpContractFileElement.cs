using Shared.Models.Areas.evolDP;

namespace Shared.Models.Areas.Finishing
{
    public class ExpContractFileElement
    {
        public string ClientName { get; set; }
        public int ContractNr { get; set; }
        public int ClientNr { get; set; }
        public int ContractID { get; set; }
        public List<FileBase> FileList { get; set; }
        public ExpContractFileElement()
        {
            FileList = new List<FileBase>();
            ClientName = string.Empty;
        }
    }
}
